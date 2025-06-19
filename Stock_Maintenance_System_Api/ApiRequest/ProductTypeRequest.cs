namespace Stock_Maintenance_System_Api.ApiRequest;
public record ProductRequest(
    string ProductName,
    int CompanyId,
    int CategoryId,
    string? Description,
    decimal Mrp,
    decimal SalesPrice,
    int TotalQuantity,
    string? TaxType = "GST",
    string? BarCode = null,
    string? BrandName = null
);