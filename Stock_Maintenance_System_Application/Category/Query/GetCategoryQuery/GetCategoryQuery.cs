using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.Category.Query.GetCategoryQuery;
public record GetCategoryQuery(int CompanyId)
    : IRequest<IResult<IReadOnlyList<KeyValuePair<string, int>>>>;