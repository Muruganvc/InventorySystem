using MediatR;
namespace Stock_Maintenance_System_Application.Customer.Query.GetAllCustomers;
public record GetAllCustomersQuery : IRequest<IReadOnlyList<GetAllCustomersQueryResponse>>;
 