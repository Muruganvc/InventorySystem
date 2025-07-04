namespace InventorySystem_Application.Product.Query.GetProducts;
public record GetProductsQueryResponse(
    int ProductId,
    string ProductName,
    int? ProductCategoryId,
    string? ProductCategoryName,
    int? CategoryId,
    string? CategoryName,
    int? CompanyId,
    string? CompanyName,
    string? Description,
    decimal MRP,
    decimal SalesPrice,
    decimal LandingPrice,
    int Quantity,
    bool IsActive,
    string? UserName
);
