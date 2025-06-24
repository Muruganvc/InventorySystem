using MediatR;
using Microsoft.EntityFrameworkCore;
using Stock_Maintenance_System_Domain.Common;

namespace Stock_Maintenance_System_Application.Customer.Query.GetAllCustomers;
internal sealed class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, IReadOnlyList<GetAllCustomersQueryResponse>>
{
    private readonly IRepository<Stock_Maintenance_System_Domain.Customer> _customerItemRepository;

    public GetAllCustomersQueryHandler(
        IRepository<Stock_Maintenance_System_Domain.Customer> customerRespository) => _customerItemRepository = customerRespository;

    public async Task<IReadOnlyList<GetAllCustomersQueryResponse>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        return await _customerItemRepository.Table
            .Select(x => new GetAllCustomersQueryResponse(
                x.CustomerId,
                x.CustomerName,
                x.Address!,
                x.Phone
            ))
            .ToListAsync(cancellationToken);
    }
}
