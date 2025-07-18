using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.Category.Command.CreateCommand;
public record CategoryCreateCommand(int CompanyId, string CategoryName, string Description,bool IsActive) :
     IRequest<IResult<int>>;
