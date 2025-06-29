using MediatR;
using Stock_Maintenance_System_Domain.Common;

namespace Stock_Maintenance_System_Application.ProductCategory.Command.UpdateCommand
{
    internal sealed class ProductCategoryUpdateCommandHandler : IRequestHandler<ProductCategoryUpdateCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Stock_Maintenance_System_Domain.ProductCategory> _productCategoryRepository;
        private readonly IRepository<Stock_Maintenance_System_Domain.Category> _categoryRepository;
        public ProductCategoryUpdateCommandHandler(IUnitOfWork unitOfWork, IRepository<Stock_Maintenance_System_Domain.ProductCategory> productCategoryRepository, IRepository<Stock_Maintenance_System_Domain.Category> categoryRepository)
        {
            _unitOfWork = unitOfWork;
            _productCategoryRepository = productCategoryRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<bool> Handle(ProductCategoryUpdateCommand request, CancellationToken cancellationToken)
        {
            var productCategory = await _productCategoryRepository.GetByAsync(a => a.ProductCategoryId == request.ProductCategoryId);
            if (productCategory == null) return false;

            var category = await _categoryRepository.GetByAsync(a => a.CategoryId == request.CategoryId);
            if (category == null) return false;
            
            productCategory.Description = request.Description;
            productCategory.CategoryId = request.CategoryId;
            productCategory.ProductCategoryName = request.CategoryName;
            productCategory.IsActive = request.IsActive;
            bool isSuccess = false;
            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                isSuccess = await _unitOfWork.SaveAsync() > 0;
            }, cancellationToken);
            return isSuccess;
        }
    }
}
