using MediatR;
using Microsoft.AspNetCore.Http;
using Stock_Maintenance_System_Domain.Common;
using System.Security.Claims;

namespace Stock_Maintenance_System_Application.ProductCategory.Command.CreateCommand
{
    internal sealed class ProductCategoryCreateCommandHandler : IRequestHandler<ProductCategoryCreateCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<Stock_Maintenance_System_Domain.Category> _categoryRepository;
        public ProductCategoryCreateCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, 
            IRepository<Stock_Maintenance_System_Domain.Category> categoryRepository)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _categoryRepository = categoryRepository;
        }

        public async Task<int> Handle(ProductCategoryCreateCommand request, CancellationToken cancellationToken)
        {
            int userId = int.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
            var category = await _categoryRepository.GetByAsync(a => a.CategoryId == request.CategoryId);
            if (category == null) return 0;

            var productCategory = new Stock_Maintenance_System_Domain.ProductCategory
            {
                CategoryId = request.CategoryId,
                Description = request.Description,
                ProductCategoryName = request.CategoryProductName,
                IsActive = request.IsActive,
                CreatedBy = userId,
                CreatedAt = DateTime.Now
            };
            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<Stock_Maintenance_System_Domain.ProductCategory>().AddAsync(productCategory);
                await _unitOfWork.SaveAsync();
            }, cancellationToken);
            return productCategory.ProductCategoryId;
        }
    }
}
