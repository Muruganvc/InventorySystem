using System.ComponentModel.DataAnnotations;

namespace Stock_Maintenance_System_Domain;
public class Customer
{
    public int CustomerId { get; set; }
    [Required]
    [MaxLength(200)]
    public string CustomerName { get; set; } = string.Empty;
    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;
    public string? Address { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    // Navigation property
    public ICollection<Orders> Orders { get; set; } = new List<Orders>();
}
