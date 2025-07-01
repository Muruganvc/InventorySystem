using MediatR;

namespace InventorySystem_Application.Category.Query.GetCategoriesQuery;
public record GetCategoriesQuery (): IRequest<IReadOnlyList<GetCategoryQueryResponse>>;
