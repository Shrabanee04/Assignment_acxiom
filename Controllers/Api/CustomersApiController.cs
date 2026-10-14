using AcxiomCRM.Data;
using AcxiomCRM.Models;
using AcxiomCRM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcxiomCRM.Controllers.Api;

[ApiController]
[Route("api/customers")]
[Authorize]
public class CustomersApiController : ControllerBase
{
    private readonly ApplicationDbContext _db; private readonly IAuditService _audit;
    public CustomersApiController(ApplicationDbContext db,IAuditService audit){_db=db;_audit=audit;}
    public record CustomerDto(int CustomerId,string CustomerCode,string CustomerName,string Email,string Phone,string? CompanyName,string Status);
    public record CreateCustomerDto(string CustomerCode,string CustomerName,string Email,string Phone,string? CompanyName,string? Address,string? City,string? State,string Status);
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> Get([FromQuery]string? q){
        var query=_db.Customers.AsNoTracking().AsQueryable();
        if(!string.IsNullOrWhiteSpace(q))query=query.Where(c=>c.CustomerName.Contains(q)||c.Email.Contains(q)||c.Phone.Contains(q));
        return await query.Select(c=>new CustomerDto(c.CustomerId,c.CustomerCode,c.CustomerName,c.Email,c.Phone,c.CompanyName,c.Status)).ToListAsync();
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CustomerDto>> GetById(int id){
        var c=await _db.Customers.FindAsync(id);if(c==null)return NotFound();
        return new CustomerDto(c.CustomerId,c.CustomerCode,c.CustomerName,c.Email,c.Phone,c.CompanyName,c.Status);
    }
    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Create(CreateCustomerDto dto){
        if(string.IsNullOrWhiteSpace(dto.CustomerName)||!new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(dto.Email)||!System.Text.RegularExpressions.Regex.IsMatch(dto.Phone??"",@"^[6-9]\d{9}$"))
            return BadRequest(new { message="Valid name, email and 10-digit Indian phone are required." });
        if(await _db.Customers.AnyAsync(c=>c.Email==dto.Email||c.Phone==dto.Phone))return Conflict(new{message="Customer email or phone already exists."});
        var c=new Customer{CustomerCode=string.IsNullOrWhiteSpace(dto.CustomerCode)?"CUS-"+Guid.NewGuid().ToString("N")[..8].ToUpper():dto.CustomerCode,CustomerName=dto.CustomerName,Email=dto.Email,Phone=dto.Phone,CompanyName=dto.CompanyName,Address=dto.Address,City=dto.City,State=dto.State,Status=dto.Status,CreatedBy=User.Identity?.Name};
        _db.Customers.Add(c);await _db.SaveChangesAsync();await _audit.LogAsync("Create","Customer",c.CustomerId.ToString(),null,c.CustomerName);
        return CreatedAtAction(nameof(GetById),new{id=c.CustomerId},new CustomerDto(c.CustomerId,c.CustomerCode,c.CustomerName,c.Email,c.Phone,c.CompanyName,c.Status));
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id,CreateCustomerDto dto){
        var c=await _db.Customers.FindAsync(id);if(c==null)return NotFound();
        if(string.IsNullOrWhiteSpace(dto.CustomerName)||!new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(dto.Email)||!System.Text.RegularExpressions.Regex.IsMatch(dto.Phone??"",@"^[6-9]\d{9}$"))return BadRequest(new{message="Invalid customer fields."});
        if(await _db.Customers.AnyAsync(x=>x.CustomerId!=id&&(x.Email==dto.Email||x.Phone==dto.Phone)))return Conflict(new{message="Email or phone already exists."});
        c.CustomerName=dto.CustomerName;c.Email=dto.Email;c.Phone=dto.Phone;c.CompanyName=dto.CompanyName;c.Address=dto.Address;c.City=dto.City;c.State=dto.State;c.Status=dto.Status;
        await _db.SaveChangesAsync();await _audit.LogAsync("Update","Customer",id.ToString(),null,c.CustomerName);return NoContent();
    }
    [HttpDelete("{id:int}")]
    [Authorize(Roles="Admin,Manager")]
    public async Task<IActionResult> Delete(int id){var c=await _db.Customers.FindAsync(id);if(c==null)return NotFound();c.Status="Inactive";await _db.SaveChangesAsync();await _audit.LogAsync("Deactivate","Customer",id.ToString(),c.CustomerName,"Inactive");return NoContent();}
}
