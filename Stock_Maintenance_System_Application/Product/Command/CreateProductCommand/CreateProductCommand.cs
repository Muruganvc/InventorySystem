using MediatR;
namespace Stock_Maintenance_System_Application.Product.Command.CreateProductCommand;
public record CreateProductCommand(
    string ProductName,
    int CompanyId,
    int CategoryId,
    int? ProductCategoryId,
    string? Description,
    decimal Mrp,
    decimal SalesPrice,
    int TotalQuantity,
    string? TaxType = "GST",
    string? BarCode = null,
    string? BrandName = null,
    int TaxPercent = 18
) : IRequest<int>;