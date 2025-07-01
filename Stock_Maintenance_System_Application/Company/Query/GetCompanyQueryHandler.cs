using MediatR;
using Microsoft.EntityFrameworkCore;
using InventorySystem_Domain.Common;

namespace InventorySystem_Application.Company.Query;

internal sealed class GetCompanyQueryHandler
    : IRequestHandler<GetCompanyQuery, IReadOnlyList<GetCompanyQueryResponse>>
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

    public async Task<IReadOnlyList<GetCompanyQueryResponse>> Handle(
        GetCompanyQuery request,
        CancellationToken cancellationToken)
    {
        var companies = await _companyRepository.Table
            .Join(
                _userRepository.Table,
                company => company.CreatedBy,
                user => user.UserId,
                (company, user) => new GetCompanyQueryResponse(
                    company.CompanyId,
                    company.CompanyName,
                    company.Description ?? string.Empty,
                    company.IsActive,
                    company.CreatedAt,
                    user.Username
                )
            )
            .ToListAsync(cancellationToken);
        return companies;
    }
}
