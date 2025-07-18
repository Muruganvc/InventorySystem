using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.Company.Command.CreateCommand;
public record CompanyCreateCommand(string CompanyName,string Description, bool IsActive)
    :IRequest<IResult<int>>;
