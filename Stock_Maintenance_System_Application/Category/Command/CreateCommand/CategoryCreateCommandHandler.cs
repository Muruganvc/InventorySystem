using InventorySystem_Application.Common;
using InventorySystem_Domain.Common;
using MediatR;

namespace InventorySystem_Application.Category.Command.CreateCommand;
internal sealed class CategoryCreateCommandHandler : IRequestHandler<CategoryCreateCommand, IResult<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserInfo _userInfo;
    private readonly IRepository<InventorySystem_Domain.Company> _companyRepository;
    private readonly IRepository<InventorySystem_Domain.Category> _categoryRepository;
    public CategoryCreateCommandHandler(IUnitOfWork unitOfWork, IUserInfo userInfo, 
        IRepository<InventorySystem_Domain.Company> companyRepository, IRepository<InventorySystem_Domain.Category> categoryRepository)
    {
        _userInfo = userInfo;
        _unitOfWork = unitOfWork;
        _companyRepository = companyRepository;
        _categoryRepository = categoryRepository;
    }
    public async Task<IResult<int>> Handle(CategoryCreateCommand request, CancellationToken cancellationToken)
    {
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
            CreatedBy = _userInfo.UserId,
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