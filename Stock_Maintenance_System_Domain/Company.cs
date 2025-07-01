using System.ComponentModel.DataAnnotations;
namespace InventorySystem_Domain;
public class Company
{
    public int CompanyId { get; set; }
    [Required, MaxLength(100)]
    public string CompanyName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int CreatedBy { get; set; }
    // Navigation Properties
    public  User CreatedByUser { get; set; } = null!;
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<Product> Product { get; set; } = new List<Product>();
}