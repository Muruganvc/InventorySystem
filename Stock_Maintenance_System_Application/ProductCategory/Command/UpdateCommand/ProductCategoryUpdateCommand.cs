using MediatR;

namespace Stock_Maintenance_System_Application.ProductCategory.Command.UpdateCommand;
public record ProductCategoryUpdateCommand(int ProductCategoryId, int CategoryId, string CategoryName, string Description, bool IsActive)
:IRequest<bool>;
