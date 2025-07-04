using MediatR;
using InventorySystem_Domain.Common;

namespace InventorySystem_Application.ProductCategory.Query.GetProductCategoryQuery;
internal sealed class GetProductCategoryQueryHandler : IRequestHandler<GetProductCategoryQuery, IReadOnlyList<KeyValuePair<string, int>>>
{
    private readonly IRepository<InventorySystem_Domain.ProductCategory> _productCategoryRepository;
    public GetProductCategoryQueryHandler(IRepository<InventorySystem_Domain.ProductCategory> productCategoryRepository) =>
        _productCategoryRepository = productCategoryRepository;
    public async Task<IReadOnlyList<KeyValuePair<string, int>>> Handle(GetProductCategoryQuery request, CancellationToken cancellationToken)
    {
        var productCategories = await _productCategoryRepository.GetListByAsync(a => a.CategoryId == request.CategoryId);
        return productCategories
            .Select(c => new KeyValuePair<string, int>(c.ProductCategoryName, c.ProductCategoryId)).OrderBy(a => a.Key)
            .ToList();
    }
}