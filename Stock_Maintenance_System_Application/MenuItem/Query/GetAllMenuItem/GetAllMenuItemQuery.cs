using MediatR;

namespace InventorySystem_Application.MenuItem.Query.GetAllMenuItem;
 
public record GetAllMenuItemQuery()
    : IRequest<IReadOnlyList<GetMenuItemQueryResponse>>;
