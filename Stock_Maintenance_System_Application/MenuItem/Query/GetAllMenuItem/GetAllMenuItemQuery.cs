using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.MenuItem.Query.GetAllMenuItem;
 
public record GetAllMenuItemQuery()
    : IRequest<IResult<IReadOnlyList<GetMenuItemQueryResponse>>>;
