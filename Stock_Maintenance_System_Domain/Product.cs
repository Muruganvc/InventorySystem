using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Stock_Maintenance_System_Domain;
public class Product
{
    public int ProductId { get; set; }
    [Required, MaxLength(100)]
    public string ProductName { get; set; }=string.Empty;
    public int CategoryId { get; set; }
    public int CompanyId { get; set; }
    public string? Description { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal MRP { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal SalesPrice { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal TaxPercent { get; set; } = 18.00m;

    [MaxLength(50)]
    public string? TaxType { get; set; }

    [MaxLength(50)]
    public string? Barcode { get; set; }

    [MaxLength(100)]
    public string? BrandName { get; set; }
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
}
