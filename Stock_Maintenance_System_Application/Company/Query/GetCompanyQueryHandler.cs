using MediatR;
using Microsoft.EntityFrameworkCore;
using InventorySystem_Domain.Common;
using InventorySystem_Application.Common;

namespace InventorySystem_Application.Company.Query;

internal sealed class GetCompanyQueryHandler
    : IRequestHandler<GetCompanyQuery, IResult<IReadOnlyList<GetCompanyQueryResponse>>>
{
    private readonly IRepository<InventorySystem_Domain.Company> _companyRepository;
    private readonly IRepository<InventorySystem_Domain.User> _userRepository;

    public GetCompanyQueryHandler(
        IRepository<InventorySystem_Domain.Company> companyRepository,
        IRepository<InventorySystem_Domain.User> userRepository)
    {
        _companyRepository = companyRepository;
        _userRepository = userRepository;
    }

    public async Task<IResult<IReadOnlyList<GetCompanyQueryResponse>>> Handle(
    GetCompanyQuery request,
    CancellationToken cancellationToken)
    {
        var query = _companyRepository.Table
            .Where(c => request.isAllActiveCompany || c.IsActive)
            .Join(
                _userRepository.Table,
                company => company.CreatedBy,
                user => user.UserId,
                (company, user) => new
                {
                    company,
                    user.Username
                }
            )
            .OrderBy(x => x.company.CompanyName);

        var companies = await query
            .Select(x => new GetCompanyQueryResponse(
                x.company.CompanyId,
                x.company.CompanyName,
                x.company.Description ?? string.Empty,
                x.company.IsActive,
                x.company.CreatedAt,
                x.Username
            ))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyList<GetCompanyQueryResponse>>.Success(companies);
    }


}
