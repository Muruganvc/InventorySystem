using MediatR;

namespace InventorySystem_Application.User.GetMenuItemPermissionQuery;
public record GetMenuItemPermissionQuery(int UserId) : IRequest<IReadOnlyList<GetMenuItemPermissionQueryResponse>>;