using MediatR;
using Microsoft.EntityFrameworkCore;
using InventorySystem_Domain.Common;
using InventorySystem_Application.Common;

namespace InventorySystem_Application.ProductCategory.Query.GetProductCategoriesQuery;

internal sealed class GetProductCategoriesQueryHandler : IRequestHandler<GetProductCategoriesQuery, IResult<IReadOnlyList<GetProductCategoryQueryResponse>>>
{
    private readonly IRepository<InventorySystem_Domain.Company> _companyRepository;
    private readonly IRepository<InventorySystem_Domain.Category> _categoryRepository;
    private readonly IRepository<InventorySystem_Domain.ProductCategory> _productCategoryRepository;
    private readonly IRepository<InventorySystem_Domain.User> _userRepository;
    public GetProductCategoriesQueryHandler(IRepository<InventorySystem_Domain.Company> companyRepository,
        IRepository<InventorySystem_Domain.Category> categoryRepository,
        IRepository<InventorySystem_Domain.ProductCategory> productCategoryRepository,
        IRepository<InventorySystem_Domain.User> userRepository)
    {
        _companyRepository = companyRepository;
        _categoryRepository = categoryRepository;
        _productCategoryRepository = productCategoryRepository;
        _userRepository = userRepository;
    }
    public async Task<IResult<IReadOnlyList<GetProductCategoryQueryResponse>>> Handle(GetProductCategoriesQuery request, CancellationToken cancellationToken)
    {
        var query = _productCategoryRepository.Table
        .Where(pc => request.isAllActive || pc.IsActive) 
        .Join(
            _categoryRepository.Table.Where(c => c.IsActive),
            pc => pc.CategoryId,
            c => c.CategoryId,
            (pc, c) => new { pc, c }
        )
        .Join(
            _companyRepository.Table.Where(co => co.IsActive),
            combined => combined.c.CompanyId,
            company => company.CompanyId,
            (combined, company) => new { combined.pc, combined.c, company }
        )
        .Join(
            _userRepository.Table,
            final => final.c.CreatedBy,
            user => user.UserId,
            (final, user) => new
            {
                final.company,
                final.c,
                final.pc,
               user.Username
            }
        )
        .OrderBy(x => x.pc.ProductCategoryName);

        var result = await query
            .Select(x => new GetProductCategoryQueryResponse(
                x.company.CompanyId,
                x.company.CompanyName,
                x.c.CategoryId,
                x.c.CategoryName,
                x.pc.ProductCategoryId,
                x.pc.ProductCategoryName,
                x.pc.Description ?? string.Empty,
                x.pc.IsActive,
                x.pc.CreatedAt,
                x.Username
            ))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyList<GetProductCategoryQueryResponse>>.Success(result);
    }

}
