using AcxiomCRM.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcxiomCRM.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _db;
    public DashboardController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        ViewBag.Customers = await _db.Customers.CountAsync();
        ViewBag.Leads = await _db.Leads.CountAsync();
        ViewBag.OpenOpportunities = await _db.Opportunities.CountAsync(o => o.Status == "Open");
        ViewBag.Won = await _db.Opportunities.CountAsync(o => o.Stage == "Won");
        ViewBag.Lost = await _db.Opportunities.CountAsync(o => o.Stage == "Lost");
        ViewBag.Pipeline = await _db.Opportunities.Where(o => o.Status == "Open").SumAsync(o => (decimal?)o.Amount) ?? 0;
        ViewBag.LeadLabels = System.Text.Json.JsonSerializer.Serialize(await _db.Leads.GroupBy(l => l.Status).Select(g => new { label = g.Key, count = g.Count() }).ToListAsync());
        ViewBag.OpportunityLabels = System.Text.Json.JsonSerializer.Serialize(await _db.Opportunities.GroupBy(o => o.Stage).Select(g => new { label = g.Key, count = g.Count() }).ToListAsync());
        return View();
    }
}
