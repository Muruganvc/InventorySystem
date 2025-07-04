using MediatR;

namespace InventorySystem_Application.Dashboard.Query.CompanyWiseIncomeQuery;

public record CompanyWiseIncomeQuery():IRequest<IReadOnlyList<CompanyWiseIncomeQueryResponse>>;
