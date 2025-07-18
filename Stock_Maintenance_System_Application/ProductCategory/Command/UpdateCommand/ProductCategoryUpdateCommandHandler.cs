using MediatR;
using InventorySystem_Domain.Common;
using InventorySystem_Application.Common;

namespace InventorySystem_Application.ProductCategory.Command.UpdateCommand
{
    internal sealed class ProductCategoryUpdateCommandHandler : IRequestHandler<ProductCategoryUpdateCommand, IResult<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<InventorySystem_Domain.ProductCategory> _productCategoryRepository;
        private readonly IRepository<InventorySystem_Domain.Category> _categoryRepository;
        public ProductCategoryUpdateCommandHandler(IUnitOfWork unitOfWork, IRepository<InventorySystem_Domain.ProductCategory> productCategoryRepository, IRepository<InventorySystem_Domain.Category> categoryRepository)
        {
            _unitOfWork = unitOfWork;
            _productCategoryRepository = productCategoryRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IResult<bool>> Handle(ProductCategoryUpdateCommand request, CancellationToken cancellationToken)
        {
            var productCategory = await _productCategoryRepository.GetByAsync(a => a.ProductCategoryId == request.ProductCategoryId);
            if (productCategory == null)
                return Result<bool>.Failure("Selected product category not found"); 

            var category = await _categoryRepository.GetByAsync(a => a.CategoryId == request.CategoryId);
            if (category == null) return Result<bool>.Failure("Selected category not found");

            productCategory.Description = request.Description;
            productCategory.CategoryId = request.CategoryId;
            productCategory.ProductCategoryName = request.ProductCategoryName;
            productCategory.IsActive = request.IsActive;
            bool isSuccess = false;
            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                isSuccess = await _unitOfWork.SaveAsync() > 0;
            }, cancellationToken);

            return Result<bool>.Success(isSuccess);
        }
    }
}
