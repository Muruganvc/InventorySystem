using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem_Domain;
public class Product
{
    public int ProductId { get; set; }
    [Required, MaxLength(100)]
    public string ProductName { get; set; }=string.Empty;
    public int CategoryId { get; set; }
    public int CompanyId { get; set; }
    public int? ProductCategoryId { get; set; }  
    public string? Description { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal MRP { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal SalesPrice { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal LandingPrice { get; set; }
    public int Quantity { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? UpdatedBy { get; set; } 

    // Navigation Properties
    public Category Category { get; set; } = null!;
    public Company Company { get; set; } = null!;
    public User CreatedByUser { get; set; } = null!;
    public User? UpdatedByUser { get; set; }
    public ProductCategory? ProductCategory { get; set; } = null!;
 
    [NotMapped]
    public string ComputedProductName =>
        $"{Company?.CompanyName ?? ""} {Category?.CategoryName ?? ""} {ProductName}".Trim();
 
 }
