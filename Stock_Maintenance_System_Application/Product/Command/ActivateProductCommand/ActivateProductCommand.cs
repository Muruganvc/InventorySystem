using MediatR;

namespace Stock_Maintenance_System_Application.Product.Command.ActivateProductCommand;
public record ActivateProductCommand(int ProductId) :IRequest<bool>;
 