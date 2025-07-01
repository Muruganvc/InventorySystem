using MediatR;

namespace InventorySystem_Application.ProductCategory.Query.GetProductCategoriesQuery;
public record GetProductCategoriesQuery() : IRequest<IReadOnlyList<GetProductCategoryQueryResponse>>;


