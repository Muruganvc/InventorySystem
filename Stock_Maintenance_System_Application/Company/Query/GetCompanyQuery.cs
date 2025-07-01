using MediatR;
namespace Stock_Maintenance_System_Application.Company.Query;
public record GetCompanyQuery(string? CompanyName)
    : IRequest<IReadOnlyList<GetCompanyQueryResponse>>;
