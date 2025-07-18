using InventorySystem_Application.Common;
using MediatR;
namespace InventorySystem_Application.Customer.Query.GetAllCustomers;
public record GetAllCustomersQuery : IRequest<IResult<IReadOnlyList<GetAllCustomersQueryResponse>>>;
 