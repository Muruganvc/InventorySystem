using MediatR;
using InventorySystem_Domain.Common;
using InventorySystem_Application.Common;

namespace InventorySystem_Application.Category.Command.UpdateCommand;
internal sealed class CategoryUpdateCommandHandler : IRequestHandler<CategoryUpdateCommand, IResult<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<InventorySystem_Domain.Company> _companyRepository;
    private readonly IRepository<InventorySystem_Domain.Category> _categoryRepository;

    public CategoryUpdateCommandHandler(IUnitOfWork unitOfWork, IRepository<InventorySystem_Domain.Company> companyRepository,
        IRepository<InventorySystem_Domain.Category> categoryRepository)
    {
        _unitOfWork= unitOfWork;
        _companyRepository= companyRepository;
        _categoryRepository= categoryRepository;
    }
    public async Task<IResult<bool>> Handle(CategoryUpdateCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByAsync(a => a.CategoryId == request.CategoryId);
        if (category == null)
            return Result<bool>.Failure("Selected category not found.");

        var company = await _companyRepository.GetByAsync(a => a.CompanyId == request.CompanyId);
        if (company == null) 
            return Result<bool>.Failure("Selected company not found.");

        category.Description = request.Description;
        category.CompanyId = request.CompanyId;
        category.CategoryName= request.CategoryName;
        category.IsActive= request.IsActive;
        bool isSuccess = false;
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            isSuccess = await _unitOfWork.SaveAsync() > 0;
        }, cancellationToken);
        return Result<bool>.Success(isSuccess); 
    }
}
