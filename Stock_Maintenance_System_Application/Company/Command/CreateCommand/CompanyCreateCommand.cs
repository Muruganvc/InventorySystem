using MediatR;

namespace InventorySystem_Application.Company.Command.CreateCommand;
public record CompanyCreateCommand(string CompanyName,string Description, bool IsActive)
    :IRequest<int>;
