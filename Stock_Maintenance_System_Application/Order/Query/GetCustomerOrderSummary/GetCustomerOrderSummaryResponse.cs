namespace Stock_Maintenance_System_Application.Order.Query.GetCustomerOrderSummary;
public record GetCustomerOrderSummaryResponse(
    int OrderId,
    string CustomerName,
    string Phone,
    string Address,
    DateTime OrderDate,
    decimal TotalAmount,
    decimal FinalAmount,
    decimal BalanceAmount
);