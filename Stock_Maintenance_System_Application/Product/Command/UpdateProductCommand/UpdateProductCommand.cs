using MediatR;

namespace Stock_Maintenance_System_Application.Product.Command.UpdateProductCommand;
public record UpdateProductCommand(
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
) : IRequest<bool>;