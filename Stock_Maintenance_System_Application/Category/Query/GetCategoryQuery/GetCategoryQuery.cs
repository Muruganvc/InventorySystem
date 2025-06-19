using MediatR;

namespace Stock_Maintenance_System_Application.Category.Query.GetCategoryQuery;
public record GetCategoryQuery(int CompanyId)
    : IRequest<IReadOnlyList<KeyValuePair<string, int>>>;