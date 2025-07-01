using MediatR;

namespace InventorySystem_Application.Category.Command.UpdateCommand;
public record CategoryUpdateCommand(int CategoryId, int CompanyId, string CategoryName, string Description,bool IsActive)
    :IRequest<bool>;
