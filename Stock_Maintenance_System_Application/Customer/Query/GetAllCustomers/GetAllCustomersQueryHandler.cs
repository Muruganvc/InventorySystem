using MediatR;
using Microsoft.EntityFrameworkCore;
using InventorySystem_Domain.Common;
using InventorySystem_Application.Common;

namespace InventorySystem_Application.Customer.Query.GetAllCustomers;
internal sealed class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, IResult<IReadOnlyList<GetAllCustomersQueryResponse>>>
{
    private readonly IRepository<InventorySystem_Domain.Customer> _customerItemRepository;

    public GetAllCustomersQueryHandler(
        IRepository<InventorySystem_Domain.Customer> customerRespository) => _customerItemRepository = customerRespository;

    public async Task<IResult<IReadOnlyList<GetAllCustomersQueryResponse>>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        var result = await _customerItemRepository.Table
            .Select(x => new GetAllCustomersQueryResponse(
                x.CustomerId,
                x.CustomerName,
                 x.Phone,
                x.Address!
            ))
            .ToListAsync(cancellationToken);
        return Result<IReadOnlyList<GetAllCustomersQueryResponse>>.Success(result.OrderBy(a => a.CustomerName).ToList());
    }
}
