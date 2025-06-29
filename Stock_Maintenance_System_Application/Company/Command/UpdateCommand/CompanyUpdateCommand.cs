using MediatR;

namespace Stock_Maintenance_System_Application.Company.Command.UpdateCommand;
public record CompanyUpdateCommand(int CompanyId,string CompanyName, string Description, bool IsActive)
    :IRequest<bool>;