using MediatR;

namespace Stock_Maintenance_System_Application.User.GetMenuItemPermissionQuery;
public record GetMenuItemPermissionQuery(int UserId) : IRequest<IReadOnlyList<GetMenuItemPermissionQueryResponse>>;