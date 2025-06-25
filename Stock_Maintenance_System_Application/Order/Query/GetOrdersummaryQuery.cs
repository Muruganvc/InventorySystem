using MediatR;

namespace Stock_Maintenance_System_Application.Order.Query;
public record GetOrdersummaryQuery() : IRequest<IReadOnlyList<GetOrderSummaryResponse>>;
 