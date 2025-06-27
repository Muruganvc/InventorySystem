using MediatR;
using Microsoft.AspNetCore.Http;
using Stock_Maintenance_System_Domain.Common;
using System.Security.Claims;

namespace Stock_Maintenance_System_Application.Product.Command.CreateProductCommand;
internal sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Stock_Maintenance_System_Domain.ProductCategory> _productRepository;
    public CreateProductCommandHandler(IUnitOfWork unitOfWork,
        IRepository<Stock_Maintenance_System_Domain.ProductCategory> productRepository,
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

        if (productCategoryId is null or 0)
        {
            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var newProductCategory = new Stock_Maintenance_System_Domain.ProductCategory
                {
                    CategoryId = request.CategoryId,
                    ProductCategoryName = request.ProductName,
                    CreatedBy = userId
                };

                await _unitOfWork.Repository<Stock_Maintenance_System_Domain.ProductCategory>().AddAsync(newProductCategory);
                await _unitOfWork.SaveAsync();

                productCategoryId = newProductCategory.ProductCategoryId;
            }, cancellationToken);
        }

        // Create the new product
        var product = new Stock_Maintenance_System_Domain.Product
        {
            ProductName = request.ProductName,
            BrandName = request.BrandName,
            Barcode = request.BarCode,
            Description = request.Description,
            CompanyId = request.CompanyId,
            CategoryId = request.CategoryId,
            ProductCategoryId = productCategoryId,
            MRP = request.Mrp,
            SalesPrice = request.SalesPrice,
            Quantity = request.TotalQuantity,
            TaxType = request.TaxType,
            TaxPercent = request.TaxPercent,
            IsActive = request.IsActive,
            CreatedAt = DateTime.Now,
            CreatedBy = userId
        };

        // Save the product
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            await _unitOfWork.Repository<Stock_Maintenance_System_Domain.Product>().AddAsync(product);
            await _unitOfWork.SaveAsync();
        }, cancellationToken);

        return product.ProductId;
    }
}
