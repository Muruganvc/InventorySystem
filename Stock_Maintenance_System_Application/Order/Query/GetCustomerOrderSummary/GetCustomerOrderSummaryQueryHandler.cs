using MediatR;
using Microsoft.EntityFrameworkCore;
using Stock_Maintenance_System_Domain.Common;
namespace Stock_Maintenance_System_Application.Order.Query.GetCustomerOrderSummary;
internal sealed class GetCustomerOrderSummaryQueryHandler
    : IRequestHandler<GetCustomerOrderSummaryQuery, IReadOnlyList<GetCustomerOrderSummaryResponse>>
{
    private readonly IRepository<Stock_Maintenance_System_Domain.Order> _orderRepository;
    private readonly IRepository<Stock_Maintenance_System_Domain.Customer> _customerRepository;
    public GetCustomerOrderSummaryQueryHandler(
        IRepository<Stock_Maintenance_System_Domain.Order> orderRepository,
        IRepository<Stock_Maintenance_System_Domain.Customer> customerRepository)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
    }
    public async Task<IReadOnlyList<GetCustomerOrderSummaryResponse>> Handle(
        GetCustomerOrderSummaryQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _orderRepository.Table
            .Join(
                _customerRepository.Table,
                ord => ord.CustomerId,
                cus => cus.CustomerId,
                (ord, cus) => new GetCustomerOrderSummaryResponse(
                    ord.OrderId,
                    cus.CustomerName,
                    cus.Phone,
                    cus.Address!,
                    ord.OrderDate,
                    ord.TotalAmount ?? 0,
                    ord.FinalAmount ?? 0,
                    ord.BalanceAmount ?? 0
                )
            )
            .ToListAsync(cancellationToken);
        return result;
    }
}
