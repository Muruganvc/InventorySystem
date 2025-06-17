using MediatR;
namespace Stock_Maintenance_System_Application.User.GetUsersQuery;
public record GetUsersQuery() :IRequest<IReadOnlyList<GetUsersQueryResponse>>;

