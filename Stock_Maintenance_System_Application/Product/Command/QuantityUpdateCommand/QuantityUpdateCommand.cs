using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.Product.Command.QuantityUpdateCommand;
public record QuantityUpdateCommand(int ProductId, int Quantity) : IRequest<IResult<bool>>;