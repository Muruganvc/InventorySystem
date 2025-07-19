using Microsoft.EntityFrameworkCore;
using InventorySystem_Domain;
using System.Text.Json;
using InventorySystem_Domain.Common;

namespace InventorySystem_Infrastructure;

public class SmsDbContext : DbContext
{
    private readonly IUserInfo _userInfo;
    public SmsDbContext(DbContextOptions<SmsDbContext> options, IUserInfo userInfo) : base(options)
    {
        _userInfo = userInfo;
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<UserRole> userRoles => Set<UserRole>();
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<UserMenuPermission> UserMenuPermissions { get; set; }
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
    public DbSet<Product> Product => Set<Product>();
    public DbSet<Customer> Customer => Set<Customer>();
    public DbSet<Order> Order => Set<Order>();
    public DbSet<OrderItem> OrderItem => Set<OrderItem>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var auditEntries = OnBeforeSaveChanges();
        var result = await base.SaveChangesAsync(cancellationToken);
        await OnAfterSaveChangesAsync(auditEntries, cancellationToken);
        return result;
    }

    private async Task OnAfterSaveChangesAsync(List<AuditLog> auditLogs, CancellationToken cancellationToken)
    {
        if (auditLogs.Any())
        {
            AuditLogs.AddRange(auditLogs);
            await base.SaveChangesAsync(cancellationToken);
        }
    }

    private List<AuditLog> OnBeforeSaveChanges()
    {
        ChangeTracker.DetectChanges();
        var auditLogs = new List<AuditLog>();

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is AuditLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                continue;

            var auditLog = new AuditLog
            {
                TableName = entry.Entity.GetType().Name,
                Action = entry.State.ToString(),
                ChangedAt = DateTime.UtcNow,
                ChangedBy = _userInfo.UserName
            };

            var keyValues = entry.Properties
                .Where(p => p.Metadata.IsPrimaryKey())
                .ToDictionary(p => p.Metadata.Name, p => p.CurrentValue);

            auditLog.KeyValues = JsonSerializer.Serialize(keyValues);

            if (entry.State == EntityState.Modified)
            {
                var modifiedProperties = entry.Properties
                    .Where(p => p.IsModified && p.OriginalValue?.ToString() != p.CurrentValue?.ToString())
                    .ToList();

                if (modifiedProperties.Count == 0)
                    continue; // Skip if no actual value changed

                var oldValues = modifiedProperties.ToDictionary(p => p.Metadata.Name, p => p.OriginalValue);
                var newValues = modifiedProperties.ToDictionary(p => p.Metadata.Name, p => p.CurrentValue);

                auditLog.OldValues = JsonSerializer.Serialize(oldValues);
                auditLog.NewValues = JsonSerializer.Serialize(newValues);
            }
            else if (entry.State == EntityState.Added)
            {
                var newValues = entry.CurrentValues.Properties
                    .ToDictionary(p => p.Name, p => entry.CurrentValues[p]);

                auditLog.NewValues = JsonSerializer.Serialize(newValues);
            }
            else if (entry.State == EntityState.Deleted)
            {
                var oldValues = entry.OriginalValues.Properties
                    .ToDictionary(p => p.Name, p => entry.OriginalValues[p]);

                auditLog.OldValues = JsonSerializer.Serialize(oldValues);
            }

            auditLogs.Add(auditLog);
        }

        return auditLogs;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<MenuItem>().ToTable("MenuItems");
        modelBuilder.Entity<UserMenuPermission>().ToTable("UserMenuPermissions");
        modelBuilder.Entity<Company>().ToTable("Company");
        modelBuilder.Entity<ProductCategory>().ToTable("ProductCategory");
        modelBuilder.Entity<Product>().ToTable("Product");
        modelBuilder.Entity<Customer>().ToTable("Customers");
        modelBuilder.Entity<Order>().ToTable("Orders");
        modelBuilder.Entity<OrderItem>().ToTable("OrderItems");
        modelBuilder.Entity<UserRole>().ToTable("UserRole");
        modelBuilder.Entity<AuditLog>().ToTable("AuditLogs");

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

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany()
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.CreatedByUser)
            .WithMany()
            .HasForeignKey(oi => oi.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.UpdatedByUser)
            .WithMany()
            .HasForeignKey(oi => oi.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .Property(o => o.OrderDate)
            .HasDefaultValueSql("GETDATE()");

        modelBuilder.Entity<Customer>()
            .Property(c => c.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        modelBuilder.Entity<OrderItem>()
            .Property(oi => oi.CreatedAt)
            .HasDefaultValueSql("GETDATE()");
    }
}
