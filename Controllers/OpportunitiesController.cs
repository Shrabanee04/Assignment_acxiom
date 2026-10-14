using AcxiomCRM.Data;
using AcxiomCRM.Models;
using AcxiomCRM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcxiomCRM.Controllers;
[Authorize]
public class OpportunitiesController : Controller
{
    private readonly ApplicationDbContext _db; private readonly IAuditService _audit;
    public OpportunitiesController(ApplicationDbContext db,IAuditService audit){_db=db;_audit=audit;}
    public async Task<IActionResult> Index(string? q){var query=_db.Opportunities.AsQueryable();if(!string.IsNullOrWhiteSpace(q))query=query.Where(o=>o.OpportunityName.Contains(q)||o.Stage.Contains(q));ViewBag.Query=q;return View(await query.OrderByDescending(o=>o.CreatedDate).ToListAsync());}
    public IActionResult Create()=>View(new Opportunity());
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Create(Opportunity model){
        if(model.ExpectedCloseDate.Date<DateTime.Today && model.Status=="Open")ModelState.AddModelError(nameof(model.ExpectedCloseDate),"Expected Close Date cannot be in the past.");
        if(model.Amount<=0)ModelState.AddModelError(nameof(model.Amount),"Opportunity Amount must be greater than 0.");
        if(model.Probability<0||model.Probability>100)ModelState.AddModelError(nameof(model.Probability),"Probability must be between 0 and 100.");
        if(!ModelState.IsValid)return View(model);model.CreatedDate=DateTime.UtcNow;_db.Opportunities.Add(model);await _db.SaveChangesAsync();
        await _audit.LogAsync("Create","Opportunity",model.OpportunityId.ToString(),null,model.OpportunityName);return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Edit(int id){var x=await _db.Opportunities.FindAsync(id);return x==null?NotFound():View(x);}
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id,Opportunity model){
        if(id!=model.OpportunityId)return BadRequest();
        if(model.ExpectedCloseDate.Date<DateTime.Today&&model.Status=="Open")ModelState.AddModelError(nameof(model.ExpectedCloseDate),"Expected Close Date cannot be in the past.");
        if(model.Amount<=0)ModelState.AddModelError(nameof(model.Amount),"Opportunity Amount must be greater than 0.");
        if(model.Probability<0||model.Probability>100)ModelState.AddModelError(nameof(model.Probability),"Probability must be between 0 and 100.");
        if(!ModelState.IsValid)return View(model);_db.Update(model);await _db.SaveChangesAsync();await _audit.LogAsync("Update","Opportunity",id.ToString(),null,model.OpportunityName);return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int id){var x=await _db.Opportunities.FindAsync(id);return x==null?NotFound():View(x);}
    [HttpPost,ActionName("Delete"),ValidateAntiForgeryToken] public async Task<IActionResult> DeleteConfirmed(int id){var x=await _db.Opportunities.FindAsync(id);if(x==null)return NotFound();_db.Opportunities.Remove(x);await _db.SaveChangesAsync();await _audit.LogAsync("Delete","Opportunity",id.ToString(),x.OpportunityName,null);return RedirectToAction(nameof(Index));}
}
