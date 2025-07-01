using MediatR;
namespace InventorySystem_Application.Company.Query;
public record GetCompanyQuery(string? CompanyName)
    : IRequest<IReadOnlyList<GetCompanyQueryResponse>>;
