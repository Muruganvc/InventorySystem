using MediatR;
using Microsoft.AspNetCore.Http;
using InventorySystem_Domain.Common;
using System.Security.Claims;
using InventorySystem_Application.Common;

namespace InventorySystem_Application.Category.Command.CreateCommand;
internal sealed class CategoryCreateCommandHandler : IRequestHandler<CategoryCreateCommand, IResult<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRepository<InventorySystem_Domain.Company> _companyRepository;
    private readonly IRepository<InventorySystem_Domain.Category> _categoryRepository;
    public CategoryCreateCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, 
        IRepository<InventorySystem_Domain.Company> companyRepository, IRepository<InventorySystem_Domain.Category> categoryRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
        _companyRepository = companyRepository;
        _categoryRepository = categoryRepository;
    }
    public async Task<IResult<int>> Handle(CategoryCreateCommand request, CancellationToken cancellationToken)
    {
        int userId = int.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
        var isExistscompany = await _companyRepository.GetByAsync(a => a.CompanyId == request.CompanyId);
        if (isExistscompany == null)
            return Result<int>.Failure("Selected company not found.");

        var isExistsCategory = await _categoryRepository.GetByAsync(a => a.CompanyId == request.CompanyId && a.CategoryName == request.CategoryName);
        if (isExistsCategory != null)
            return Result<int>.Failure("Entered company and category already exists.");

        var category = new InventorySystem_Domain.Category
        {
            CategoryName = request.CategoryName,
            CompanyId = request.CompanyId,
            Description = request.Description,
            IsActive = request.IsActive,
            CreatedBy = userId,
            CreatedAt = DateTime.Now
        };
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            await _unitOfWork.Repository<InventorySystem_Domain.Category>().AddAsync(category);
            await _unitOfWork.SaveAsync();
        }, cancellationToken);
        return Result<int>.Success(isExistscompany.CompanyId); 
    }
}