using MediatR;
using Stock_Maintenance_System_Domain.Common;

namespace Stock_Maintenance_System_Application.Category.Query.GetCategoryQuery;
internal sealed class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, IReadOnlyList<KeyValuePair<string, int>>>
{
    private readonly IRepository<Stock_Maintenance_System_Domain.Category> _categoryRepository;
    public GetCategoryQueryHandler(IRepository<Stock_Maintenance_System_Domain.Category> categoryRepository) => _categoryRepository = categoryRepository;
    public async Task<IReadOnlyList<KeyValuePair<string, int>>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetListByAsync(a => a.CompanyId == request.CompanyId);
        return categories
            .Select(c => new KeyValuePair<string, int>(c.CategoryName, c.CategoryId))
            .ToList();
    }
}