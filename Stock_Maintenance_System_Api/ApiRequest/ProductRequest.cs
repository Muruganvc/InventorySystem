namespace InventorySystem_Api.ApiRequest;
public record ProductRequest(
    string ProductName,
    int CompanyId,
    int CategoryId,
    int? ProductCategoryId,
    string? Description,
    decimal Mrp,
    decimal SalesPrice,
    decimal LandingPrice,
    int TotalQuantity,
    bool IsActive = false
);