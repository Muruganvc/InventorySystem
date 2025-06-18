using MediatR;

namespace Stock_Maintenance_System_Application.MenuItem.Query.GetAllMenuItem;
 
public record GetAllMenuItemQuery()
    : IRequest<IReadOnlyList<GetMenuItemQueryResponse>>;
