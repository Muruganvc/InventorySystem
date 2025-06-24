namespace Stock_Maintenance_System_Api.ApiRequest;
public record ProductRequest(
    string ProductName,
    int CompanyId,
    int CategoryId,
    int? ProductCategoryId,
    string? Description,
    decimal Mrp,
    decimal SalesPrice,
    int TotalQuantity,
    bool IsActive = false,
    string? TaxType = "GST",
    string? BarCode = null,
    string? BrandName = null,
    int TaxPercent = 18
);