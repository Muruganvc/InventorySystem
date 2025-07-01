using MediatR;
using Microsoft.AspNetCore.Http;
using InventorySystem_Domain.Common;
using System.Security.Claims;

namespace InventorySystem_Application.Category.Command.CreateCommand;
internal sealed class CategoryCreateCommandHandler : IRequestHandler<CategoryCreateCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRepository<InventorySystem_Domain.Company> _companyRepository;
    public CategoryCreateCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IRepository<InventorySystem_Domain.Company> companyRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
        _companyRepository = companyRepository;
    }
    public async Task<int> Handle(CategoryCreateCommand request, CancellationToken cancellationToken)
    {
        int userId = int.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
        var company = await _companyRepository.GetByAsync(a => a.CompanyId == request.CompanyId);
        if (company == null) return 0;

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
        return company.CompanyId;
    }
}