using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.Product.Command.UpdateProductCommand;
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
   decimal LandingPrice
) : IRequest<IResult<bool>>;