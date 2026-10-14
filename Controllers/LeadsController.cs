using AcxiomCRM.Data;
using AcxiomCRM.Models;
using AcxiomCRM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcxiomCRM.Controllers;
[Authorize]
public class LeadsController : Controller
{
    private readonly ApplicationDbContext _db; private readonly IAuditService _audit;
    public LeadsController(ApplicationDbContext db, IAuditService audit) { _db=db; _audit=audit; }
    public async Task<IActionResult> Index(string? q) {
        var query=_db.Leads.AsQueryable();
        if(!string.IsNullOrWhiteSpace(q)) query=query.Where(x=>x.LeadName.Contains(q)||(x.CompanyName??"").Contains(q)||x.Status.Contains(q));
        ViewBag.Query=q; return View(await query.OrderByDescending(x=>x.CreatedDate).ToListAsync());
    }
    public IActionResult Create()=>View(new Lead{LeadCode="LEAD-"+Guid.NewGuid().ToString("N")[..8].ToUpper()});
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Create(Lead model) {
        if(!ModelState.IsValid)return View(model);
        model.CreatedDate=DateTime.UtcNow; _db.Leads.Add(model); await _db.SaveChangesAsync();
        await _audit.LogAsync("Create","Lead",model.LeadId.ToString(),null,model.LeadName); return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Edit(int id){var x=await _db.Leads.FindAsync(id);return x==null?NotFound():View(x);}
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id,Lead model){
        if(id!=model.LeadId)return BadRequest(); if(!ModelState.IsValid)return View(model);
        _db.Update(model); await _db.SaveChangesAsync(); await _audit.LogAsync("Update","Lead",id.ToString(),null,model.LeadName); return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int id){var x=await _db.Leads.FindAsync(id);return x==null?NotFound():View(x);}
    [HttpPost,ActionName("Delete"),ValidateAntiForgeryToken] public async Task<IActionResult> DeleteConfirmed(int id){
        var x=await _db.Leads.FindAsync(id);if(x==null)return NotFound();_db.Leads.Remove(x);await _db.SaveChangesAsync();
        await _audit.LogAsync("Delete","Lead",id.ToString(),x.LeadName,null);return RedirectToAction(nameof(Index));
    }
}
