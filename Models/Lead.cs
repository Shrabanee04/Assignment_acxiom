using System.ComponentModel.DataAnnotations;

namespace AcxiomCRM.Models;

public class Lead
{
    public int LeadId { get; set; }
    [Required, StringLength(30)] public string LeadCode { get; set; } = "";
    [Required, StringLength(120)] public string LeadName { get; set; } = "";
    [EmailAddress, StringLength(150)] public string? Email { get; set; }
    [RegularExpression(@"^$|^[6-9]\d{9}$", ErrorMessage = "Enter a valid 10-digit Indian mobile number.")]
    public string? Phone { get; set; }
    [StringLength(120)] public string? CompanyName { get; set; }
    [Required] public string Source { get; set; } = "Website";
    [Required] public string Status { get; set; } = "New";
    [Required] public string Priority { get; set; } = "Medium";
    [Range(0, 1000000000)] public decimal ExpectedValue { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public string? AssignedTo { get; set; }
}
