using MediatR;
using Stock_Maintenance_System_Domain.Common;

namespace Stock_Maintenance_System_Application.Company.Query;
internal sealed class GetCompanyQueryHandler : IRequestHandler<GetCompanyQuery, IReadOnlyList<KeyValuePair<string, int>>>
{
    private readonly IRepository<Stock_Maintenance_System_Domain.Company.Company> _companyRepository;
    public GetCompanyQueryHandler(IRepository<Stock_Maintenance_System_Domain.Company.Company> companyRepository) => _companyRepository = companyRepository;
    public async Task<IReadOnlyList<KeyValuePair<string, int>>> Handle(GetCompanyQuery request, CancellationToken cancellationToken)
    {
        var companies = await _companyRepository.GetListByAsync(
            c => string.IsNullOrEmpty(request.CompanyName) || c.CompanyName.Contains(request.CompanyName));
        return companies
            .Select(c => new KeyValuePair<string, int>(c.CompanyName, c.CompanyId))
            .ToList();
    }
}