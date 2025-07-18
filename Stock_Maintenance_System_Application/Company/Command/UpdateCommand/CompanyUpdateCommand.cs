using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.Company.Command.UpdateCommand;
public record CompanyUpdateCommand(int CompanyId,string CompanyName, string Description, bool IsActive)
    :IRequest<IResult<bool>>;