using InventorySystem_Application.Common;
using InventorySystem_Domain.Common;
using MediatR;

namespace InventorySystem_Application.Company.Command.BulkCompanyCommand;

internal sealed class BulkCompanyCommandHandler : IRequestHandler<BulkCompanyCommand, IResult<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<InventorySystem_Domain.Company> _companyRepository;
    private readonly IRepository<InventorySystem_Domain.Category> _categoryRepository;
    private readonly IRepository<InventorySystem_Domain.ProductCategory> _productCategoryRepository;
    private readonly IUserInfo _userInfo;

    public BulkCompanyCommandHandler(
        IUnitOfWork unitOfWork,
        IRepository<InventorySystem_Domain.Company> companyRepository,
        IRepository<InventorySystem_Domain.Category> categoryRepository,
        IRepository<InventorySystem_Domain.ProductCategory> productCategoryRepository,
        IUserInfo userInfo)
    {
        _unitOfWork = unitOfWork;
        _companyRepository = companyRepository;
        _categoryRepository = categoryRepository;
        _productCategoryRepository = productCategoryRepository; 
        _userInfo = userInfo;
    }

    public async Task<IResult<bool>> Handle(BulkCompanyCommand request, CancellationToken cancellationToken)
    {
        

        bool isSuccess = false;

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            foreach (var item in request.BulkCompanyCommands)
            {
                // Add or get Company
                var existingCompany = await _companyRepository.GetByAsync(c => c.CompanyName == item.CompanyName);
                int companyId;

                if (existingCompany == null)
                {
                    var company = new InventorySystem_Domain.Company
                    {
                        CompanyName = item.CompanyName,
                        Description = null,
                        IsActive = false,
                        CreatedBy = _userInfo.UserId,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _companyRepository.AddAsync(company);
                    await _unitOfWork.SaveAsync();
                    companyId = company.CompanyId;
                }
                else
                {
                    companyId = existingCompany.CompanyId;
                }

                // Add or get Category
                var existingCategory = await _categoryRepository.GetByAsync(c =>
                    c.CategoryName == item.CategoryName && c.CompanyId == companyId);
                int categoryId;

                if (existingCategory == null)
                {
                    var category = new InventorySystem_Domain.Category
                    {
                        CategoryName = item.CategoryName,
                        CompanyId = companyId,
                        Description = null,
                        IsActive = false,
                        CreatedBy = _userInfo.UserId,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _categoryRepository.AddAsync(category);
                    await _unitOfWork.SaveAsync();
                    categoryId = category.CategoryId;
                }
                else
                {
                    categoryId = existingCategory.CategoryId;
                }

                // Add ProductCategory if not exists
                var existingProductCategory = await _productCategoryRepository.GetByAsync(pc =>
                    pc.ProductCategoryName == item.ProductCategory && pc.CategoryId == categoryId);

                if (existingProductCategory == null)
                {
                    var productCategory = new InventorySystem_Domain.ProductCategory
                    {
                        ProductCategoryName = item.ProductCategory,
                        CategoryId = categoryId,
                        Description = null,
                        IsActive = false,
                        CreatedBy = _userInfo.UserId,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _productCategoryRepository.AddAsync(productCategory);
                    isSuccess = await _unitOfWork.SaveAsync() > 0;
                }
            }
        }, cancellationToken);
        return Result<bool>.Success(isSuccess);
    }
}
