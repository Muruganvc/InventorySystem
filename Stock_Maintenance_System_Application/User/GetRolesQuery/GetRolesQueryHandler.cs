using InventorySystem_Application.Common;
using InventorySystem_Application.User.GetUserQuery;
using InventorySystem_Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem_Application.User.GetRolesQuery;
internal sealed class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, IResult<IReadOnlyList<GetRolesQueryResponse>>>
{
    private readonly IRepository<InventorySystem_Domain.Role> _roleRepository;
        public GetRolesQueryHandler(IRepository<InventorySystem_Domain.Role> roleRepository) =>
        _roleRepository = roleRepository;
    public async Task<IResult<IReadOnlyList<GetRolesQueryResponse>>> Handle(
        GetRolesQuery request,
        CancellationToken cancellationToken)
    {
        var roles = await _roleRepository.Table.ToListAsync(cancellationToken);
        var response = roles
            .Select(role => new GetRolesQueryResponse(role.RoleId, role.Name))
            .ToList();
        return Result<IReadOnlyList<GetRolesQueryResponse>>.Success(response);
    }
}
