using MediatR;

namespace InventorySystem_Application.Order.Query;
public record GetOrdersummaryQuery(int OrderId) : IRequest<IReadOnlyList<GetOrderSummaryResponse>>;
 