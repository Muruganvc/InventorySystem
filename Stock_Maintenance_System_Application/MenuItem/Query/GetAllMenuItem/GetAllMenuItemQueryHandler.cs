using MediatR;
using Microsoft.EntityFrameworkCore;
using InventorySystem_Domain.Common;
using InventorySystem_Application.Common;

namespace InventorySystem_Application.MenuItem.Query.GetAllMenuItem;
internal sealed class GetAllMenuItemQueryHandler : IRequestHandler<GetAllMenuItemQuery, IResult<IReadOnlyList<GetMenuItemQueryResponse>>>
{
    private readonly IRepository<InventorySystem_Domain.MenuItem> _menuItemRepository;

    public GetAllMenuItemQueryHandler(
        IRepository<InventorySystem_Domain.MenuItem> menuItemRepository) => _menuItemRepository = menuItemRepository;

    public async Task<IResult<IReadOnlyList<GetMenuItemQueryResponse>>> Handle(GetAllMenuItemQuery request, CancellationToken cancellationToken)
    {
        var allMenuItems = await _menuItemRepository.Table.ToListAsync(cancellationToken);
        var response = BuildMenuTree(allMenuItems, null);
        return Result<IReadOnlyList<GetMenuItemQueryResponse>>.Success(response); 
    }
    private List<GetMenuItemQueryResponse> BuildMenuTree(List<InventorySystem_Domain.MenuItem> allItems, int? parentId)
    {
        return allItems
            .Where(m => m.ParentId == parentId)
            .Select(m => new GetMenuItemQueryResponse
            {
                Id = m.Id,
                Label = m.Label,
                Icon = m.Icon,
                Route = m.Route,
                SubMenuItem = BuildMenuTree(allItems, m.Id)
            })
            .ToList();
    }
}