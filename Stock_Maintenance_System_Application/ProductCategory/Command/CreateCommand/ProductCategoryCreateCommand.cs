using MediatR;

namespace Stock_Maintenance_System_Application.ProductCategory.Command.CreateCommand;

public record ProductCategoryCreateCommand(int CategoryId, string CategoryProductName, string Description, bool IsActive) : IRequest<int>;