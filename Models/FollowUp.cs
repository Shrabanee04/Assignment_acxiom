using System.ComponentModel.DataAnnotations;

namespace AcxiomCRM.Models;

public class FollowUp
{
    public int FollowUpId { get; set; }
    public int? CustomerId { get; set; }
    public int? LeadId { get; set; }
    [DataType(DataType.Date), CustomValidation(typeof(FollowUp), nameof(ValidateDate))]
    public DateTime FollowUpDate { get; set; } = DateTime.Today.AddDays(1);
    [Required] public string FollowUpType { get; set; } = "Call";
    [StringLength(500)] public string? Remarks { get; set; }
    [Required] public string Status { get; set; } = "Planned";
    public string? AssignedTo { get; set; }
    public static ValidationResult? ValidateDate(DateTime date, ValidationContext context) =>
        date.Date < DateTime.Today ? new ValidationResult("Follow-up date cannot be earlier than today.") : ValidationResult.Success;
}
