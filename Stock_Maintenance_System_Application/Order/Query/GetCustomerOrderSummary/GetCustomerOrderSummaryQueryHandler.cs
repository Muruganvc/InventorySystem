using MediatR;
using Microsoft.EntityFrameworkCore;
using InventorySystem_Domain.Common;
namespace InventorySystem_Application.Order.Query.GetCustomerOrderSummary;
internal sealed class GetCustomerOrderSummaryQueryHandler
    : IRequestHandler<GetCustomerOrderSummaryQuery, IReadOnlyList<GetCustomerOrderSummaryResponse>>
{
    private readonly IRepository<InventorySystem_Domain.Order> _orderRepository;
    private readonly IRepository<InventorySystem_Domain.Customer> _customerRepository;
    public GetCustomerOrderSummaryQueryHandler(
        IRepository<InventorySystem_Domain.Order> orderRepository,
        IRepository<InventorySystem_Domain.Customer> customerRepository)
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
        return result.OrderBy(a => a.CustomerName).ToList();
    }
}
