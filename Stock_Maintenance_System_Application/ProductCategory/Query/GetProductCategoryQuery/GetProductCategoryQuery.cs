using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.ProductCategory.Query.GetProductCategoryQuery;
public record GetProductCategoryQuery(int CategoryId)
    : IRequest<IResult<IReadOnlyList<KeyValuePair<string, int>>>>;
