using MediatR;
using Microsoft.EntityFrameworkCore;
using Stock_Maintenance_System_Domain.Common;

namespace Stock_Maintenance_System_Application.Category.Query.GetCategoriesQuery;
internal sealed class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IReadOnlyList<GetCategoryQueryResponse>>
{
    private readonly IRepository<Stock_Maintenance_System_Domain.Company> _companyRepository;
    private readonly IRepository<Stock_Maintenance_System_Domain.Category> _categoryRepository;
    private readonly IRepository<Stock_Maintenance_System_Domain.User> _userRepository;
    public GetCategoriesQueryHandler(IRepository<Stock_Maintenance_System_Domain.Company> companyRepository,
        IRepository<Stock_Maintenance_System_Domain.Category> categoryRepository,
        IRepository<Stock_Maintenance_System_Domain.User> userRepository)
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
        return result;
    }
}
