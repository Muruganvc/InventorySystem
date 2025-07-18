using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.MenuItem.Query;
public record GetMenuItemQuery(int UserId)
    : IRequest<IResult<IReadOnlyList<GetMenuItemQueryResponse>>>;
