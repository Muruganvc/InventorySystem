using MediatR;

namespace Stock_Maintenance_System_Application.ProductCategory.Query.GetProductCategoryQuery;
public record GetProductCategoryQuery(int CategoryId)
    : IRequest<IReadOnlyList<KeyValuePair<string, int>>>;
