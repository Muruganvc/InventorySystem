using MediatR;
namespace Stock_Maintenance_System_Application.ProductCompany.Query;
public record GetProductCompanyQuery(int companyId)
    : IRequest<IReadOnlyList<GetProductCompanyQueryResponse>>;
