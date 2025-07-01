using MediatR;
using Microsoft.EntityFrameworkCore;
using InventorySystem_Domain.Common;

namespace InventorySystem_Application.MenuItem.Query;
internal sealed class GetMenuItemQueryHandler : IRequestHandler<GetMenuItemQuery, IReadOnlyList<GetMenuItemQueryResponse>>
{
    private readonly IRepository<InventorySystem_Domain.UserMenuPermission> _userMenuPermissionRepository;
    private readonly IRepository<InventorySystem_Domain.MenuItem> _menuItemRepository;
    public GetMenuItemQueryHandler(
        IRepository<InventorySystem_Domain.UserMenuPermission> userMenuPermissionRepository,
        IRepository<InventorySystem_Domain.MenuItem> menuItemRepository)
    {
        _userMenuPermissionRepository = userMenuPermissionRepository;
        _menuItemRepository = menuItemRepository;
    }
    public async Task<IReadOnlyList<GetMenuItemQueryResponse>> Handle(GetMenuItemQuery request, CancellationToken cancellationToken)
    {
        var menuItems = await _menuItemRepository.Table
            .Join(
                _userMenuPermissionRepository.Table,
                menu => menu.Id,
                perm => perm.MenuItemId,
                (menu, perm) => new { menu, perm }
            )
            .Where(x => x.perm.UserId == request.UserId)
            .Select(x => x.menu)
            .ToListAsync(cancellationToken);

        var response = BuildMenuTree(menuItems, null);
        return response;
    }

    private List<GetMenuItemQueryResponse> BuildMenuTree(List<InventorySystem_Domain.MenuItem> allItems, int? parentId)
    {
        return allItems
            .Where(m => m.ParentId == parentId)
            .Select(m => new GetMenuItemQueryResponse
            {
                Id = m.Id,
                ParentId = m.ParentId ?? 0,
                Label = m.Label,
                Icon = m.Icon,
                Route = m.Route,
                SubMenuItem = BuildMenuTree(allItems, m.Id)
            }).OrderBy(x => x.OrderBy)
            .ToList();
    }
}