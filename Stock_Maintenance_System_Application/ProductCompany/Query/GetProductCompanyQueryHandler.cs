using MediatR;
using Stock_Maintenance_System_Domain.Common;

namespace Stock_Maintenance_System_Application.ProductCompany.Query;
internal sealed class GetProductCompanyQueryHandler : IRequestHandler<GetProductCompanyQuery, IReadOnlyList<GetProductCompanyQueryResponse>>
{
    private readonly IRepository<Stock_Maintenance_System_Domain.ProductCompany.ProductCompany> _productCompanyRepository;
    public GetProductCompanyQueryHandler(IRepository<Stock_Maintenance_System_Domain.ProductCompany.ProductCompany> productCompanyRepository) =>
        _productCompanyRepository = productCompanyRepository;
    public async Task<IReadOnlyList<GetProductCompanyQueryResponse>> Handle(GetProductCompanyQuery request, CancellationToken cancellationToken)
    {
        var companies = await _productCompanyRepository.GetListByAsync(a => a.CompanyId == request.companyId);
        return companies
            .Select(c => new GetProductCompanyQueryResponse(
                c.ProductCompanyId,
                c.CompanyId,
                c.ProductName))
            .ToList();
    }
}