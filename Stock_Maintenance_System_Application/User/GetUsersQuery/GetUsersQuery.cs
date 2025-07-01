using MediatR;
namespace InventorySystem_Application.User.GetUsersQuery;
public record GetUsersQuery() :IRequest<IReadOnlyList<GetUsersQueryResponse>>;

