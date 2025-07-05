using MediatR;
using Microsoft.AspNetCore.Http;
using InventorySystem_Domain.Common;
using System.Security.Claims;

namespace InventorySystem_Application.Product.Command.CreateProductCommand;
internal sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<InventorySystem_Domain.Product> _productRepository;
    public CreateProductCommandHandler(IUnitOfWork unitOfWork,
        IRepository<InventorySystem_Domain.Product> productRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        int userId = int.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
        // Determine product category ID (create new if needed)
        int? productCategoryId = request.ProductCategoryId;

        var isExistProduct = await _productRepository.GetByAsync(a => a.ProductCategoryId == request.ProductCategoryId && a.CategoryId == request.CategoryId && a.CompanyId == request.CompanyId);
        if (isExistProduct is not null) return 0;

        if (productCategoryId is null or 0)
        {
            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var newProductCategory = new InventorySystem_Domain.ProductCategory
                {
                    CategoryId = request.CategoryId,
                    ProductCategoryName = request.ProductName,
                    CreatedBy = userId
                };

                await _unitOfWork.Repository<InventorySystem_Domain.ProductCategory>().AddAsync(newProductCategory);
                await _unitOfWork.SaveAsync();

                productCategoryId = newProductCategory.ProductCategoryId;
            }, cancellationToken);
        }

        // Create the new product
        var product = new InventorySystem_Domain.Product
        {
            ProductName = request.ProductName,
            Description = request.Description,
            CompanyId = request.CompanyId,
            CategoryId = request.CategoryId,
            ProductCategoryId = productCategoryId,
            MRP = request.Mrp,
            LandingPrice = request.LandingPrice,
            SalesPrice = request.SalesPrice,
            Quantity = request.TotalQuantity,
            IsActive = request.IsActive,
            CreatedAt = DateTime.Now,
            CreatedBy = userId,
            SerialNo = request.SerialNo,
        };

        // Save the product
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            await _unitOfWork.Repository<InventorySystem_Domain.Product>().AddAsync(product);
            await _unitOfWork.SaveAsync();
        }, cancellationToken);

        return product.ProductId;
    }
}
