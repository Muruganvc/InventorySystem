using MediatR;
using Microsoft.AspNetCore.Http;
using InventorySystem_Domain.Common;
using System.Security.Claims;
using InventorySystem_Application.Common;

namespace InventorySystem_Application.ProductCategory.Command.CreateCommand
{
    internal sealed class ProductCategoryCreateCommandHandler : IRequestHandler<ProductCategoryCreateCommand, IResult<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<InventorySystem_Domain.Category> _categoryRepository;
        private readonly IUserInfo _userInfo;
        public ProductCategoryCreateCommandHandler(IUnitOfWork unitOfWork, 
            IRepository<InventorySystem_Domain.Category> categoryRepository, IUserInfo userInfo)
        {
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
            _userInfo = userInfo;
        }

        public async Task<IResult<int>> Handle(ProductCategoryCreateCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByAsync(a => a.CategoryId == request.CategoryId);
            if (category == null) return Result<int>.Failure("Selected category not found");

            var productCategory = new InventorySystem_Domain.ProductCategory
            {
                CategoryId = request.CategoryId,
                Description = request.Description,
                ProductCategoryName = request.CategoryProductName,
                IsActive = request.IsActive,
                CreatedBy = _userInfo.UserId,
                CreatedAt = DateTime.Now
            };
            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<InventorySystem_Domain.ProductCategory>().AddAsync(productCategory);
                await _unitOfWork.SaveAsync();
            }, cancellationToken);
            return Result<int>.Success(productCategory.ProductCategoryId);
        }
    }
}
