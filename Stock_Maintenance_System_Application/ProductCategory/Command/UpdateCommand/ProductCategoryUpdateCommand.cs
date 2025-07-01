using MediatR;

namespace InventorySystem_Application.ProductCategory.Command.UpdateCommand;
public record ProductCategoryUpdateCommand(int ProductCategoryId, int CategoryId, string ProductCategoryName, string Description, bool IsActive)
:IRequest<bool>;
