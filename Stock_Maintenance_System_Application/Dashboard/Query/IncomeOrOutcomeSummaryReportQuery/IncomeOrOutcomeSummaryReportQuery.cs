using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.Dashboard.Query.IncomeOrOutcomeSummaryReportQuery;
public record IncomeOrOutcomeSummaryReportQuery(DateTime? FromDate, DateTime? EndDate) 
    :IRequest<IResult<IReadOnlyList<IncomeOrOutcomeSummaryReportQueryResponse>>>;
 