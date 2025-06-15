namespace Stock_Maintenance_System_Domain.Product;
public class Product
{
    public int ProductId { get; set; }
    public string ProductIdName { get; set; } = null!;
    public string ProductCompany { get; set; } = null!;
    public string? ProductModel { get; set; }
    public decimal? Mrp { get; set; }
    public decimal? SalePrice { get; set; }
    public int Quantity { get; set; }
    public int TotalQuantity { get; set; }
    public DateTime? PurchaseDate { get; set; } = DateTime.Now;
    public bool IsActive { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public int? ModifiedBy { get; set; }
    public DateTime ModifiedDate { get; set; } = DateTime.Now;
    public User.User? Creator { get; set; }
    public User.User? Modifier { get; set; }

    // This property is computed by the database
    public string ItemName => $"{ProductIdName} {ProductCompany}";
}
