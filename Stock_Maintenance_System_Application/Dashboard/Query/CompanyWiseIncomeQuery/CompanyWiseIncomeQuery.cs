using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.Dashboard.Query.CompanyWiseIncomeQuery;

public record CompanyWiseIncomeQuery():IRequest<IResult<IReadOnlyList<CompanyWiseIncomeQueryResponse>>>;
