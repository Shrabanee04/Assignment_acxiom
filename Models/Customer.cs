using System.ComponentModel.DataAnnotations;

namespace AcxiomCRM.Models;

public class Customer
{
    public int CustomerId { get; set; }

    [Required]
    [StringLength(30)]
    public string CustomerCode { get; set; } = "";

    [Required(ErrorMessage = "Customer name is required.")]
    [StringLength(120)]
    public string CustomerName { get; set; } = "";

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    [StringLength(150)]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(
        @"^[6-9]\d{9}$",
        ErrorMessage = "Enter a valid 10-digit Indian mobile number."
    )]
    public string Phone { get; set; } = "";

    [StringLength(120)]
    public string? CompanyName { get; set; }

    [StringLength(250)]
    public string? Address { get; set; }

    [StringLength(80)]
    public string? City { get; set; }

    [StringLength(80)]
    public string? State { get; set; }

    [Required]
    public string Status { get; set; } = "Active";

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public string? CreatedBy { get; set; }

    // Username of the Sales Executive assigned to this customer.
    [StringLength(256)]
    public string? AssignedTo { get; set; }
}
