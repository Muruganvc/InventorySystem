using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.User.GetMenuItemPermissionQuery;
public record GetMenuItemPermissionQuery(int UserId) :
    IRequest<IResult<IReadOnlyList<GetMenuItemPermissionQueryResponse>>>;