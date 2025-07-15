using MediatR;
using Microsoft.EntityFrameworkCore;
using InventorySystem_Domain.Common;

namespace InventorySystem_Application.Category.Query.GetCategoriesQuery;
internal sealed class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IReadOnlyList<GetCategoryQueryResponse>>
{
    private readonly IRepository<InventorySystem_Domain.Company> _companyRepository;
    private readonly IRepository<InventorySystem_Domain.Category> _categoryRepository;
    private readonly IRepository<InventorySystem_Domain.User> _userRepository;
    public GetCategoriesQueryHandler(IRepository<InventorySystem_Domain.Company> companyRepository,
        IRepository<InventorySystem_Domain.Category> categoryRepository,
        IRepository<InventorySystem_Domain.User> userRepository)
    {
        _companyRepository = companyRepository;
        _categoryRepository = categoryRepository;
        _userRepository = userRepository;
    }
    public async Task<IReadOnlyList<GetCategoryQueryResponse>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var result = await _companyRepository.Table
    .Join(_categoryRepository.Table,
        company => company.CompanyId,
        category => category.CompanyId,
        (company, category) => new { company, category })
    .Where(a => a.category.IsActive && a.company.IsActive)
    .Join(_userRepository.Table,
        combined => combined.category.CreatedBy,
        user => user.UserId,
        (combined, user) => new GetCategoryQueryResponse(
            combined.company.CompanyId,
            combined.company.CompanyName,
            combined.category.CategoryId,
            combined.category.CategoryName,
            combined.category.Description ?? string.Empty,
            combined.category.IsActive,
            combined.category.CreatedAt,
            user.Username
        ))
    .ToListAsync(cancellationToken);
        return result.OrderBy(a => a.CategoryName).ToList();
    }
}
