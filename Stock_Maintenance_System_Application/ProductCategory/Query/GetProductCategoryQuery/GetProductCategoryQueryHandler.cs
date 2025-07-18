using MediatR;
using InventorySystem_Domain.Common;
using InventorySystem_Application.Common;

namespace InventorySystem_Application.ProductCategory.Query.GetProductCategoryQuery;
internal sealed class GetProductCategoryQueryHandler : IRequestHandler<GetProductCategoryQuery, IResult<IReadOnlyList<KeyValuePair<string, int>>>>
{
    private readonly IRepository<InventorySystem_Domain.ProductCategory> _productCategoryRepository;
    public GetProductCategoryQueryHandler(IRepository<InventorySystem_Domain.ProductCategory> productCategoryRepository) =>
        _productCategoryRepository = productCategoryRepository;
    public async Task<IResult<IReadOnlyList<KeyValuePair<string, int>>>> Handle(GetProductCategoryQuery request, CancellationToken cancellationToken)
    {
        var productCategories = await _productCategoryRepository.GetListByAsync(a => a.CategoryId == request.CategoryId && a.IsActive);
        var result = productCategories
            .Select(c => new KeyValuePair<string, int>(c.ProductCategoryName, c.ProductCategoryId)).OrderBy(a => a.Key)
            .ToList();

        return Result<IReadOnlyList<KeyValuePair<string, int>>>.Success(result);
    }
}