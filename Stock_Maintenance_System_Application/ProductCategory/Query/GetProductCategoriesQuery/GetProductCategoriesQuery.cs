using MediatR;

namespace Stock_Maintenance_System_Application.ProductCategory.Query.GetProductCategoriesQuery;
public record GetProductCategoriesQuery() : IRequest<IReadOnlyList<GetProductCategoryQueryResponse>>;


