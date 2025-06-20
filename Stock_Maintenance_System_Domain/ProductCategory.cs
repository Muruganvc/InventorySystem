namespace Stock_Maintenance_System_Domain;

public class ProductCategory
{
    public int ProductCategoryId { get; set; }
    public string ProductCategoryName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int CreatedBy { get; set; }

    // Navigation properties
    public virtual Category Category { get; set; } = default!;
    public virtual User CreatedByUser { get; set; } = default!;
    public virtual ICollection<Product> Product { get; } = default!;
}