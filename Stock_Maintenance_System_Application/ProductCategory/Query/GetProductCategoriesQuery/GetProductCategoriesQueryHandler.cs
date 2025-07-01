using MediatR;
using Microsoft.EntityFrameworkCore;
using Stock_Maintenance_System_Domain.Common;

namespace Stock_Maintenance_System_Application.ProductCategory.Query.GetProductCategoriesQuery;

internal sealed class GetProductCategoriesQueryHandler : IRequestHandler<GetProductCategoriesQuery, IReadOnlyList<GetProductCategoryQueryResponse>>
{
    private readonly IRepository<Stock_Maintenance_System_Domain.Company> _companyRepository;
    private readonly IRepository<Stock_Maintenance_System_Domain.Category> _categoryRepository;
    private readonly IRepository<Stock_Maintenance_System_Domain.ProductCategory> _productCategoryRepository;
    private readonly IRepository<Stock_Maintenance_System_Domain.User> _userRepository;
    public GetProductCategoriesQueryHandler(IRepository<Stock_Maintenance_System_Domain.Company> companyRepository,
        IRepository<Stock_Maintenance_System_Domain.Category> categoryRepository,
        IRepository<Stock_Maintenance_System_Domain.ProductCategory> productCategoryRepository,
        IRepository<Stock_Maintenance_System_Domain.User> userRepository)
    {
        _companyRepository = companyRepository;
        _categoryRepository = categoryRepository;
        _productCategoryRepository = productCategoryRepository;
        _userRepository = userRepository;
    }
    public async Task<IReadOnlyList<GetProductCategoryQueryResponse>> Handle(GetProductCategoriesQuery request, CancellationToken cancellationToken)
    {
        var result = await _productCategoryRepository.Table
            .Join(_categoryRepository.Table,
                pc => pc.CategoryId,
                c => c.CategoryId,
                (pc, c) => new { pc, c })
            .Join(_companyRepository.Table,
                combined => combined.c.CompanyId,
                company => company.CompanyId,
                (combined, company) => new { combined.pc, combined.c, company })
            .Join(_userRepository.Table,
                final => final.c.CreatedBy,
                user => user.UserId,
                (final, user) => new GetProductCategoryQueryResponse(
                    final.company.CompanyId,
                    final.company.CompanyName,
                    final.c.CategoryId,
                    final.c.CategoryName,
                    final.pc.ProductCategoryId,
                    final.pc.ProductCategoryName,
                    final.pc.Description ?? string.Empty,
                    final.pc.IsActive,
                    final.pc.CreatedAt,
                    user.Username
                ))
            .ToListAsync(cancellationToken);

        return result;
    }

}
