using MediatR;
namespace InventorySystem_Application.Customer.Query.GetAllCustomers;
public record GetAllCustomersQuery : IRequest<IReadOnlyList<GetAllCustomersQueryResponse>>;
 