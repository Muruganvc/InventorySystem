using MediatR;

namespace InventorySystem_Application.Category.Command.CreateCommand;
public record CategoryCreateCommand(int CompanyId, string CategoryName, string Description,bool IsActive) : IRequest<int>;
