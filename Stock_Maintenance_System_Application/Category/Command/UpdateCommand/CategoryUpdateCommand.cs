using MediatR;

namespace Stock_Maintenance_System_Application.Category.Command.UpdateCommand;
public record CategoryUpdateCommand(int CategoryId, int CompanyId, string CategoryName, string Description,bool IsActive)
    :IRequest<bool>;
