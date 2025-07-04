namespace InventorySystem_Application.Dashboard.Query.CompanyWiseIncomeQuery;
public record CompanyWiseIncomeQueryResponse(
    string CompanyName,
    string CategoryName,
    string ProductCategoryName,
    int TotalQuantity,
    decimal Income
);
