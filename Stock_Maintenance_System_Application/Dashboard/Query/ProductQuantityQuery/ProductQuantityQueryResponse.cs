namespace InventorySystem_Application.Dashboard.Query.ProductQuantityQuery;

public record ProductQuantityQueryResponse(string CompanyName, string CategoryName,
    string ProductCategoryName, int Quantity);
