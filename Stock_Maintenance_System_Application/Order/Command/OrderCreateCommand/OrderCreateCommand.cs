using MediatR;

namespace Stock_Maintenance_System_Application.Order.Command.OrderCreateCommand;

public record CustomerCommand(
    int CustomerId,
    string CustomerName,
    string Phone,
    string? Address
);

public record OrderItemCommand(
    int ProductId,
    int Quantity,
    decimal UnitPrice,
    decimal DiscountPercent,
    string? Remarks
);

public record OrderCreateCommand(CustomerCommand Customer, List<OrderItemCommand> OrderItemRequests,decimal BalanceAmount) : IRequest<int>;


