using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.ProductCategory.Query.GetProductCategoriesQuery;
public record GetProductCategoriesQuery(bool isAllActive) : 
    IRequest<IResult<IReadOnlyList<GetProductCategoryQueryResponse>>>;


