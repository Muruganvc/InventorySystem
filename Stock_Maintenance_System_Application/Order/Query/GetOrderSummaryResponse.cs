namespace Stock_Maintenance_System_Application.Order.Query;
public class GetOrderSummaryResponse
{
    public string FullProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal SubTotal { get; set; }
    public decimal NetTotal { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal FinalAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal BalanceAmount { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string User { get; set; } = string.Empty;
}
