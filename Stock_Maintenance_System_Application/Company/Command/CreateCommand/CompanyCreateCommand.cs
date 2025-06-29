using MediatR;

namespace Stock_Maintenance_System_Application.Company.Command.CreateCommand;
public record CompanyCreateCommand(string CompanyName,string Description, bool IsActive)
    :IRequest<int>;
