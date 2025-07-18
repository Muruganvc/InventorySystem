using MediatR;
using Microsoft.EntityFrameworkCore;
using InventorySystem_Domain.Common;
using InventorySystem_Application.Common;

namespace InventorySystem_Application.Category.Query.GetCategoriesQuery;
internal sealed class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IResult<IReadOnlyList<GetCategoryQueryResponse>>>
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
    public async Task<IResult<IReadOnlyList<GetCategoryQueryResponse>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var query = _companyRepository.Table
            .Where(c => c.IsActive)
            .Join(
                _categoryRepository.Table,
                company => company.CompanyId,
                category => category.CompanyId,
                (company, category) => new { company, category }
            )
            .Where(x => request.isAllActive || x.category.IsActive)
            .Join(
                _userRepository.Table,
                combined => combined.category.CreatedBy,
                user => user.UserId,
                (combined, user) => new
                {
                    combined.company,
                    combined.category,
                    user.Username
                }
            )
            .OrderBy(x => x.category.CategoryName);

        var result = await query
            .Select(x => new GetCategoryQueryResponse(
                x.company.CompanyId,
                x.company.CompanyName,
                x.category.CategoryId,
                x.category.CategoryName,
                x.category.Description ?? string.Empty,
                x.category.IsActive,
                x.category.CreatedAt,
                x.Username
            ))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyList<GetCategoryQueryResponse>>.Success(result);
    }

}
