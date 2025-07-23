using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.User.GetUserRoleQuery;
public record GetUserRoleQuery() : IRequest<IResult<IReadOnlyList<GetUserRoleQueryResponse>>>;
