using MediatR;

namespace InventorySystem_Application.Product.Command.ActivateProductCommand;
public record ActivateProductCommand(int ProductId) :IRequest<bool>;
 