using System.ComponentModel.DataAnnotations;
namespace InventorySystem_Domain;
public class Category
{
    public int CategoryId { get; set; }
    [Required, MaxLength(100)]
    public string CategoryName { get; set; } =string.Empty;
    public int? CompanyId { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int CreatedBy { get; set; }
    // Navigation Properties
    public Company? Company { get; set; }
    public User CreatedByUser { get; set; } = null!;
    public ICollection<Product> Product { get; set; } = new List<Product>();
    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
}