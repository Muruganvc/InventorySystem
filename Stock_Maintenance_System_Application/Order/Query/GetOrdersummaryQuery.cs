using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.Order.Query;
public record GetOrdersummaryQuery(int OrderId) : IRequest<IResult<IReadOnlyList<GetOrderSummaryResponse>>>;
 