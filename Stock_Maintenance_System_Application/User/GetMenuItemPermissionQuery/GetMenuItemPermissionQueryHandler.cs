using MediatR;
using Microsoft.EntityFrameworkCore;
using InventorySystem_Domain;
using InventorySystem_Domain.Common;
using InventorySystem_Application.Common;

namespace InventorySystem_Application.User.GetMenuItemPermissionQuery
{
    internal sealed class GetMenuItemPermissionQueryHandler : IRequestHandler<GetMenuItemPermissionQuery, IResult<IReadOnlyList<GetMenuItemPermissionQueryResponse>>>
    {
        private readonly IRepository<InventorySystem_Domain.MenuItem> _menuItemRepository;
        private readonly IRepository<UserMenuPermission> _userMenuRepository;
        public GetMenuItemPermissionQueryHandler(IRepository<InventorySystem_Domain.MenuItem> menuItemRepository,
            IRepository<UserMenuPermission> userMenuRepository)
        {
            _menuItemRepository = menuItemRepository;
            _userMenuRepository = userMenuRepository;
        }
        public async Task<IResult<IReadOnlyList<GetMenuItemPermissionQueryResponse>>> Handle(GetMenuItemPermissionQuery request, CancellationToken cancellationToken)
        {
            var result = await _menuItemRepository.Table
            .GroupJoin(
                    _userMenuRepository.Table.Where(mp => mp.UserId == request.UserId),
                    mi => mi.Id,
                    mp => mp.MenuItemId,
                    (mi, mpGroup) => new GetMenuItemPermissionQueryResponse
                    {
                        Id = mi.Id,
                        Label = mi.Label,
                        Icon = mi.Icon,
                        HasPermission = mpGroup.Any()
                    }
                ).ToListAsync(cancellationToken);

            return Result<IReadOnlyList<GetMenuItemPermissionQueryResponse>>.Success(result); 
        }
    }
}
