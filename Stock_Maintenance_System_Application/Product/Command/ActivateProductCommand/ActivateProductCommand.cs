using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.Product.Command.ActivateProductCommand;
public record ActivateProductCommand(int ProductId) :IRequest<IResult<bool>>;
 