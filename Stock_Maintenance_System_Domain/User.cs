namespace InventorySystem_Domain;
public class User
{
    public int UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? Email { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime PasswordLastChanged { get; set; }
    public DateTime PasswordExpiresAt => PasswordLastChanged.AddDays(30);
    public bool IsPasswordExpired { get; set; } = false;
    public DateTime? LastLogin { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    // Navigation Properties
    public ICollection<Company> CreatedCompanies { get; set; } = new List<Company>();
    public ICollection<Category> CreatedCategories { get; set; } = new List<Category>();
    public ICollection<Product> CreatedProduct { get; set; } = new List<Product>();
    public ICollection<Product> UpdatedProduct { get; set; } = new List<Product>();
    public virtual ICollection<ProductCategory> CreatedProductCategories { get; set; } = new List<ProductCategory>();
}
