namespace Stock_Maintenance_System_Application.Product.Query.GetProducts;
public record GetProductsQueryResponse(
    int CompanyId,
    string CompanyName,
    int CategoryId,
    string CategoryName,
    int ProductId,
    string ProductName,
    string? Description,
    decimal MRP,
    decimal SalesPrice,
    decimal TaxPercent,
    int TotalQuantity,
    bool IsActive,
    string UserName
);
