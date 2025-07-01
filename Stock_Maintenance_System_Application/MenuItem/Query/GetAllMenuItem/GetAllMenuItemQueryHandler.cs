using MediatR;
using Microsoft.EntityFrameworkCore;
using Stock_Maintenance_System_Domain.Common;
namespace Stock_Maintenance_System_Application.MenuItem.Query.GetAllMenuItem;
internal sealed class GetAllMenuItemQueryHandler : IRequestHandler<GetAllMenuItemQuery, IReadOnlyList<GetMenuItemQueryResponse>>
{
    private readonly IRepository<Stock_Maintenance_System_Domain.MenuItem> _menuItemRepository;

    public GetAllMenuItemQueryHandler(
        IRepository<Stock_Maintenance_System_Domain.MenuItem> menuItemRepository) => _menuItemRepository = menuItemRepository;

    public async Task<IReadOnlyList<GetMenuItemQueryResponse>> Handle(GetAllMenuItemQuery request, CancellationToken cancellationToken)
    {
        var allMenuItems = await _menuItemRepository.Table.ToListAsync(cancellationToken);
        var response = BuildMenuTree(allMenuItems, null);
        return response;
    }
    private List<GetMenuItemQueryResponse> BuildMenuTree(List<Stock_Maintenance_System_Domain.MenuItem> allItems, int? parentId)
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