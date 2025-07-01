using MediatR;

namespace InventorySystem_Application.ProductCategory.Query.GetProductCategoryQuery;
public record GetProductCategoryQuery(int CategoryId)
    : IRequest<IReadOnlyList<KeyValuePair<string, int>>>;
