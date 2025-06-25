using MediatR;

namespace Stock_Maintenance_System_Application.Order.Query.GetCustomerOrderSummary;
public record GetCustomerOrderSummaryQuery
    : IRequest<IReadOnlyList<GetCustomerOrderSummaryResponse>>;
 