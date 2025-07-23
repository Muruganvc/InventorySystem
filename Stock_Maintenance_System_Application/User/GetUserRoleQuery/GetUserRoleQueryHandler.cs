using InventorySystem_Application.Common;
using InventorySystem_Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem_Application.User.GetUserRoleQuery;

internal sealed class GetUserRoleQueryHandler : IRequestHandler<GetUserRoleQuery, IResult<IReadOnlyList<GetUserRoleQueryResponse>>>
{
    private readonly IRepository<InventorySystem_Domain.Role> _roleRepository;
    private readonly IRepository<InventorySystem_Domain.UserRole> _userRoleRepository;
    public GetUserRoleQueryHandler(IRepository<InventorySystem_Domain.Role> roleRepository,
        IRepository<InventorySystem_Domain.UserRole> userRoleRepository)
    {
        _roleRepository = roleRepository;
        _userRoleRepository = userRoleRepository;
    }
    public async Task<IResult<IReadOnlyList<GetUserRoleQueryResponse>>> Handle(
        GetUserRoleQuery request,
        CancellationToken cancellationToken)
    {
        var userRolesWithNames = await _userRoleRepository.Table
            .Join(
                _roleRepository.Table,
                ur => ur.RoleId,
                r => r.RoleId,
                (ur, r) => new GetUserRoleQueryResponse(
                    ur.RoleId,
                    ur.UserId,
                    ur.UserRoleId,
                    r.Name
                )
            ).ToListAsync(cancellationToken);
        return Result<IReadOnlyList<GetUserRoleQueryResponse>>.Success(userRolesWithNames);
    }
}
