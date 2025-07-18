using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.Order.Query.GetCustomerOrderSummary;
public record GetCustomerOrderSummaryQuery
    : IRequest<IResult<IReadOnlyList<GetCustomerOrderSummaryResponse>>>;
 