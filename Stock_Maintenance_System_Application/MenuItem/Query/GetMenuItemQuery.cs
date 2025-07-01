using MediatR;

namespace InventorySystem_Application.MenuItem.Query;
public record GetMenuItemQuery(int UserId)
    : IRequest<IReadOnlyList<GetMenuItemQueryResponse>>;
