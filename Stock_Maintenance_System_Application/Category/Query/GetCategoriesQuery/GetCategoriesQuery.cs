using MediatR;

namespace Stock_Maintenance_System_Application.Category.Query.GetCategoriesQuery;
public record GetCategoriesQuery (): IRequest<IReadOnlyList<GetCategoryQueryResponse>>;
