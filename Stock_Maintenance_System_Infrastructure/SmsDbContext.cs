using Microsoft.EntityFrameworkCore;
using Stock_Maintenance_System_Domain; 

namespace Stock_Maintenance_System_Infrastructure;
public class SmsDbContext : DbContext
{
    public SmsDbContext(DbContextOptions<SmsDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users => Set<User>();
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<UserMenuPermission> UserMenuPermissions { get; set; }
    public DbSet<Company> Companies => Set<Company>(); 
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
    public DbSet<Product> Product => Set<Product>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<MenuItem>().ToTable("MenuItems");
        modelBuilder.Entity<UserMenuPermission>().ToTable("UserMenuPermissions");
        modelBuilder.Entity<Company>().ToTable("Company"); 
        modelBuilder.Entity<ProductCategory>().ToTable("ProductCategory");
        modelBuilder.Entity<Product>().ToTable("Product");

        modelBuilder.Entity<MenuItem>()
            .HasOne(m => m.Parent)
            .WithMany(m => m.Children)
            .HasForeignKey(m => m.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Company>()
            .HasOne(c => c.CreatedByUser)
            .WithMany(u => u.CreatedCompanies)
            .HasForeignKey(c => c.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Category>()
            .HasOne(cat => cat.Company)
            .WithMany(c => c.Categories)
            .HasForeignKey(cat => cat.CompanyId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Category>()
            .HasOne(cat => cat.CreatedByUser)
            .WithMany(u => u.CreatedCategories)
            .HasForeignKey(cat => cat.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product>()
            .HasOne(pt => pt.Company)
            .WithMany(c => c.Product)
            .HasForeignKey(pt => pt.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product>()
            .HasOne(pt => pt.Category)
            .WithMany(cat => cat.Product)
            .HasForeignKey(pt => pt.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product>()
            .HasOne(pt => pt.CreatedByUser)
            .WithMany(u => u.CreatedProduct)
            .HasForeignKey(pt => pt.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product>()
            .HasOne(pt => pt.UpdatedByUser)
            .WithMany(u => u.UpdatedProduct)
            .HasForeignKey(pt => pt.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<ProductCategory>()
            .HasOne(pc => pc.Category)
            .WithMany(c => c.ProductCategories)  
            .HasForeignKey(pc => pc.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProductCategory>()
            .HasOne(pc => pc.CreatedByUser)
            .WithMany(u => u.CreatedProductCategories)
            .HasForeignKey(pc => pc.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
