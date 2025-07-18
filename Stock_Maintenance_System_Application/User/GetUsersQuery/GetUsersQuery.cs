using InventorySystem_Application.Common;
using MediatR;
namespace InventorySystem_Application.User.GetUsersQuery;
public record GetUsersQuery() :IRequest<IResult<IReadOnlyList<GetUsersQueryResponse>>>;

