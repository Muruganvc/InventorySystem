using MediatR;

namespace InventorySystem_Application.Order.Query.GetCustomerOrderSummary;
public record GetCustomerOrderSummaryQuery
    : IRequest<IReadOnlyList<GetCustomerOrderSummaryResponse>>;
 