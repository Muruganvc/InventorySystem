using MediatR;
using InventorySystem_Domain.Common;
using InventorySystem_Application.Common;

namespace InventorySystem_Application.Category.Query.GetCategoryQuery;
internal sealed class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, IResult<IReadOnlyList<KeyValuePair<string, int>>>>
{
    private readonly IRepository<InventorySystem_Domain.Category> _categoryRepository;
    public GetCategoryQueryHandler(IRepository<InventorySystem_Domain.Category> categoryRepository) => _categoryRepository = categoryRepository;
    public async Task<IResult<IReadOnlyList<KeyValuePair<string, int>>>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetListByAsync(a => a.CompanyId == request.CompanyId && a.IsActive);
        var result= categories
            .Select(c => new KeyValuePair<string, int>(c.CategoryName, c.CategoryId)).OrderBy(a => a.Key)
            .ToList();
        return Result<IReadOnlyList<KeyValuePair<string, int>>>.Success(result);
    }
}