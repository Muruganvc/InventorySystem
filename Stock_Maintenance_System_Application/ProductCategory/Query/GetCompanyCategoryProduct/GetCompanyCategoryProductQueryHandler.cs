using InventorySystem_Application.Common;
using InventorySystem_Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem_Application.ProductCategory.Query.GetCompanyCategoryProduct;

internal sealed class GetCompanyCategoryProductQueryHandler : IRequestHandler<GetCompanyCategoryProductQuery, IResult<IReadOnlyList<KeyValuePair<string, string>>>>
{
    private readonly IRepository<InventorySystem_Domain.Category> _categoryRepository;
    private readonly IRepository<InventorySystem_Domain.Company> _companyRepository;
    private readonly IRepository<InventorySystem_Domain.ProductCategory> _productCategoryRepository;
    public GetCompanyCategoryProductQueryHandler(IRepository<InventorySystem_Domain.Category> categoryRepository,
        IRepository<InventorySystem_Domain.Company> companyRepository, IRepository<InventorySystem_Domain.ProductCategory> productCategoryRepository)
    {
        _categoryRepository = categoryRepository;
        _companyRepository = companyRepository;
        _productCategoryRepository = productCategoryRepository;
    }
    public async Task<IResult<IReadOnlyList<KeyValuePair<string, string>>>> Handle(GetCompanyCategoryProductQuery request, CancellationToken cancellationToken)
    {
        var result = await _companyRepository.Table
            .Where(com => com.IsActive)
            .Join(
                _categoryRepository.Table.Where(cat => cat.IsActive),
                com => com.CompanyId,
                cat => cat.CompanyId,
                (com, cat) => new { com, cat }
            )
            .Join(
                _productCategoryRepository.Table.Where(pc => pc.IsActive),
                x => x.cat.CategoryId,
                pc => pc.CategoryId,
                (x, pc) => new
                {
                    CompanyName = x.com.CompanyName,
                    CategoryName = x.cat.CategoryName,
                    ProductCategoryName = pc.ProductCategoryName,
                    CompositeId = $"{x.com.CompanyId}${x.cat.CategoryId}${pc.ProductCategoryId}"
                }
            )
            .ToListAsync(cancellationToken);

        var ordered = result
            .OrderBy(x => $"{x.CompanyName} {x.CategoryName} {x.ProductCategoryName}")
            .Select(x => new KeyValuePair<string, string>(
                $"{x.CompanyName} {x.CategoryName} {x.ProductCategoryName}",
                x.CompositeId
            ))
            .ToList();

        return Result<IReadOnlyList<KeyValuePair<string, string>>>.Success(ordered);
    }

}