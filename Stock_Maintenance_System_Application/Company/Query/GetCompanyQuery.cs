using InventorySystem_Application.Common;
using MediatR;
namespace InventorySystem_Application.Company.Query;
public record GetCompanyQuery(bool isAllActiveCompany, string? CompanyName)
    : IRequest<IResult<IReadOnlyList<GetCompanyQueryResponse>>>;
