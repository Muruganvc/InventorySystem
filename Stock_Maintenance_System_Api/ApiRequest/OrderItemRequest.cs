namespace Stock_Maintenance_System_Api.ApiRequest;

public class CustomerRequest
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }=string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Address { get; set; }
}
public class OrderItemRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountPercent { get; set; }
    public string? Remarks { get; set; }
}

public record OrderCreateRequest(CustomerRequest Customer, List<OrderItemRequest> OrderItemRequests, decimal BalanceAmount);
    
