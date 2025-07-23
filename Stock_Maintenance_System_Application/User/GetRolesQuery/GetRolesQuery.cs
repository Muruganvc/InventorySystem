using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.User.GetRolesQuery;
public record GetRolesQuery(): IRequest<IResult<IReadOnlyList<GetRolesQueryResponse>>>;
