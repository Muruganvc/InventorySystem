using InventorySystem_Application.Common;
using MediatR;
namespace InventorySystem_Application.Product.Command.CreateProductCommand;
public record CreateProductCommand(
    string ProductName,
    int CompanyId,
    int CategoryId,
    int? ProductCategoryId,
    string? Description,
    decimal Mrp,
    decimal SalesPrice,
    int TotalQuantity,
    decimal LandingPrice,
    bool IsActive = false
) : IRequest<IResult<int>>;