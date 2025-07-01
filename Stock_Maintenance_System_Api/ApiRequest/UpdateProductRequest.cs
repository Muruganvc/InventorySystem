namespace InventorySystem_Api.ApiRequest;
public record UpdateProductRequest(
   int ProductId,
   string ProductName,
   int CompanyId,
   int CategoryId,
   int? ProductCategoryId,
   string? Description,
   decimal Mrp,
   decimal SalesPrice,
   int TotalQuantity,
   bool IsActive,
   string? TaxType = "GST",
   string? BarCode = null,
   string? BrandName = null,
   int TaxPercent = 18
);