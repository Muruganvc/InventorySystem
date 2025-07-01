using MediatR;
using Microsoft.EntityFrameworkCore;
using Stock_Maintenance_System_Domain;
using Stock_Maintenance_System_Domain.Common;

namespace Stock_Maintenance_System_Application.User.GetMenuItemPermissionQuery
{
    internal sealed class GetMenuItemPermissionQueryHandler : IRequestHandler<GetMenuItemPermissionQuery, IReadOnlyList<GetMenuItemPermissionQueryResponse>>
    {
        private readonly IRepository<Stock_Maintenance_System_Domain.MenuItem> _menuItemRepository;
        private readonly IRepository<UserMenuPermission> _userMenuRepository;
        public GetMenuItemPermissionQueryHandler(IRepository<Stock_Maintenance_System_Domain.MenuItem> menuItemRepository,
            IRepository<UserMenuPermission> userMenuRepository)
        {
            _menuItemRepository = menuItemRepository;
            _userMenuRepository = userMenuRepository;
        }
        public async Task<IReadOnlyList<GetMenuItemPermissionQueryResponse>> Handle(GetMenuItemPermissionQuery request, CancellationToken cancellationToken)
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
            return result ?? new();
        }
    }
}
