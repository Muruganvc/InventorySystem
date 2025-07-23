namespace InventorySystem_Application.Dashboard.Query.IncomeOrOutcomeSummaryReportQuery;
public record IncomeOrOutcomeSummaryReportQueryResponse(
   int ProductId,
    string ProductName,
    decimal SalesPrice,
    decimal LandingPrice,
    decimal MRP,
    decimal AvgUnitPrice,
    int TotalQuantity,
    decimal TotalGainedAmount
);
