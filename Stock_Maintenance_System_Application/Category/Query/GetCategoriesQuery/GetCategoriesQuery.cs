using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.Category.Query.GetCategoriesQuery;
public record GetCategoriesQuery (bool isAllActive): IRequest<IResult<IReadOnlyList<GetCategoryQueryResponse>>>;
