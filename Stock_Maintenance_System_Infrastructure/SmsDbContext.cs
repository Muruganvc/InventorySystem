using Microsoft.EntityFrameworkCore;
using Stock_Maintenance_System_Domain.Company;
using Stock_Maintenance_System_Domain.Product;
using Stock_Maintenance_System_Domain.ProductCompany;
using Stock_Maintenance_System_Domain.User;
namespace Stock_Maintenance_System_Infrastructure;
public class SmsDbContext : DbContext
{
    public SmsDbContext(DbContextOptions<SmsDbContext> options) : base(options) { 
    }
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<ProductCompany> ProductCompanies => Set<ProductCompany>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Role>().ToTable("Role");
        modelBuilder.Entity<Company>().ToTable("Company");
        modelBuilder.Entity<ProductCompany>().ToTable("ProductCompany");
        modelBuilder.Entity<Product>().ToTable("Product1");

    }
}
