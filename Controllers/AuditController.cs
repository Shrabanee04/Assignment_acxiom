using AcxiomCRM.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcxiomCRM.Controllers;
[Authorize(Roles="Admin")]
public class AuditController : Controller
{
    private readonly ApplicationDbContext _db;
    public AuditController(ApplicationDbContext db)=>_db=db;
    public async Task<IActionResult> Index(string? module,string? actionName){
        var q=_db.AuditLogs.AsQueryable();
        if(!string.IsNullOrWhiteSpace(module))q=q.Where(x=>x.EntityName.Contains(module));
        if(!string.IsNullOrWhiteSpace(actionName))q=q.Where(x=>x.Action.Contains(actionName));
        ViewBag.Module=module;ViewBag.ActionName=actionName;
        return View(await q.OrderByDescending(x=>x.CreatedDate).Take(500).ToListAsync());
    }
}
