using MediatR;

namespace InventorySystem_Application.Category.Query.GetCategoryQuery;
public record GetCategoryQuery(int CompanyId)
    : IRequest<IReadOnlyList<KeyValuePair<string, int>>>;