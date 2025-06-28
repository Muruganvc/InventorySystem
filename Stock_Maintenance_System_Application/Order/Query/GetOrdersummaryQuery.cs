using MediatR;

namespace Stock_Maintenance_System_Application.Order.Query;
public record GetOrdersummaryQuery(int OrderId) : IRequest<IReadOnlyList<GetOrderSummaryResponse>>;
 