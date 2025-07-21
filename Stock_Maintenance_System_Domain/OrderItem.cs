using System.ComponentModel.DataAnnotations.Schema;

namespace InventorySystem_Domain;

public class OrderItem
{
    public int OrderItemId { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountPercent { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CreatedBy { get; set; } 
    public Order Order { get; set; } = null!;
    public Product Product { get; set; } = null!;
    public User CreatedByUser { get; set; } = null!; 

    // Computed columns (not mapped):
    [NotMapped] public decimal SubTotal => Quantity * UnitPrice;
    [NotMapped] public decimal DiscountAmount => SubTotal * DiscountPercent / 100.0m;
    [NotMapped] public decimal NetTotal => SubTotal - DiscountAmount;
    public string? SerialNo { get; set; }
}
