using MediatR;
using Stock_Maintenance_System_Domain.Common;

namespace Stock_Maintenance_System_Application.Category.Command.UpdateCommand;
internal sealed class CategoryUpdateCommandHandler : IRequestHandler<CategoryUpdateCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Stock_Maintenance_System_Domain.Company> _companyRepository;
    private readonly IRepository<Stock_Maintenance_System_Domain.Category> _categoryRepository;
    public CategoryUpdateCommandHandler(IUnitOfWork unitOfWork, IRepository<Stock_Maintenance_System_Domain.Company> companyRepository,
        IRepository<Stock_Maintenance_System_Domain.Category> categoryRepository)
    {
        _unitOfWork= unitOfWork;
        _companyRepository= companyRepository;
        _categoryRepository= categoryRepository;
    }
    public async Task<bool> Handle(CategoryUpdateCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByAsync(a => a.CategoryId == request.CategoryId);
        if (category == null) return false;

        var company = await _companyRepository.GetByAsync(a => a.CompanyId == request.CompanyId);
        if (company == null) return false;

        category.Description = request.Description;
        category.CompanyId = request.CompanyId;
        category.CategoryName= request.CategoryName;
        category.IsActive= request.IsActive;
        bool isSuccess = false;
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            isSuccess = await _unitOfWork.SaveAsync() > 0;
        }, cancellationToken);
        return isSuccess;
    }
}
