using MediatR;

namespace Stock_Maintenance_System_Application.MenuItem.Query;
public record GetMenuItemQuery(int UserId)
    : IRequest<IReadOnlyList<GetMenuItemQueryResponse>>;
