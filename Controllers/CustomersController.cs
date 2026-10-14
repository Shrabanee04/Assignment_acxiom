using AcxiomCRM.Data;
using AcxiomCRM.Models;
using AcxiomCRM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcxiomCRM.Controllers;

[Authorize]
public class CustomersController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IAuditService _audit;
    public CustomersController(ApplicationDbContext db, IAuditService audit) { _db = db; _audit = audit; }

    public async Task<IActionResult> Index(string? q) {
        var query = _db.Customers.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q)) query = query.Where(c => c.CustomerName.Contains(q) || c.Email.Contains(q) || c.Phone.Contains(q) || (c.CompanyName ?? "").Contains(q));
        ViewBag.Query = q;
        return View(await query.OrderByDescending(c => c.CreatedDate).ToListAsync());
    }
    public IActionResult Create() => View(new Customer { CustomerCode = "CUS-" + Guid.NewGuid().ToString("N")[..8].ToUpper() });
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Customer model) {
        if (await _db.Customers.AnyAsync(c => c.Email == model.Email)) ModelState.AddModelError(nameof(model.Email), "Email already exists.");
        if (await _db.Customers.AnyAsync(c => c.Phone == model.Phone)) ModelState.AddModelError(nameof(model.Phone), "Phone already exists.");
        if (!ModelState.IsValid) return View(model);
        model.CreatedBy = User.Identity?.Name;
        model.CreatedDate = DateTime.UtcNow;
        _db.Customers.Add(model); await _db.SaveChangesAsync();
        await _audit.LogAsync("Create", "Customer", model.CustomerId.ToString(), null, model.CustomerName);
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Edit(int id) {
        var item = await _db.Customers.FindAsync(id); return item == null ? NotFound() : View(item);
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Customer model) {
        if (id != model.CustomerId) return BadRequest();
        if (await _db.Customers.AnyAsync(c => c.CustomerId != id && c.Email == model.Email)) ModelState.AddModelError(nameof(model.Email), "Email already exists.");
        if (await _db.Customers.AnyAsync(c => c.CustomerId != id && c.Phone == model.Phone)) ModelState.AddModelError(nameof(model.Phone), "Phone already exists.");
        if (!ModelState.IsValid) return View(model);
        model.CreatedBy = (await _db.Customers.AsNoTracking().FirstAsync(c => c.CustomerId == id)).CreatedBy;
        _db.Update(model); await _db.SaveChangesAsync();
        await _audit.LogAsync("Update", "Customer", id.ToString(), null, model.CustomerName);
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Details(int id) { var item = await _db.Customers.FindAsync(id); return item == null ? NotFound() : View(item); }
    public async Task<IActionResult> Delete(int id) { var item = await _db.Customers.FindAsync(id); return item == null ? NotFound() : View(item); }
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id) {
        var item = await _db.Customers.FindAsync(id); if (item == null) return NotFound();
        _db.Customers.Remove(item); await _db.SaveChangesAsync();
        await _audit.LogAsync("Delete", "Customer", id.ToString(), item.CustomerName, null);
        return RedirectToAction(nameof(Index));
    }
}
