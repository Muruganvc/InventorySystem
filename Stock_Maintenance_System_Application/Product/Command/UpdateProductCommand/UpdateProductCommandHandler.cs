using MediatR;
using Microsoft.AspNetCore.Http;
using InventorySystem_Domain.Common;
using System.Security.Claims;

namespace InventorySystem_Application.Product.Command.UpdateProductCommand;
internal sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<InventorySystem_Domain.Product> _productRepository;
    public UpdateProductCommandHandler(IUnitOfWork unitOfWork,
        IRepository<InventorySystem_Domain.Product> productRepository, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        int userId = int.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
        var product = await _productRepository.GetByAsync(a => a.ProductId == request.ProductId);
        if(product == null) return false;
        product.Description = request.Description;
        product.CategoryId= request.CategoryId;
        product.ProductCategoryId = request.ProductCategoryId;
        product.ProductName = request.ProductName;
        product.MRP = request.Mrp;
        product.Barcode = request.BarCode;
        product.TaxPercent= request.TaxPercent;
        product.SalesPrice= request.SalesPrice;
        product.TaxType= request.TaxType;
        product.Quantity = request.TotalQuantity;
        product.BrandName = request.BrandName;
        product.CompanyId = request.CompanyId;
        product.UpdatedBy = userId;
        product.UpdatedAt= DateTime.Now;
        product.Description= request.Description;
        product.IsActive= request.IsActive;
        bool isSuccess = false;
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            isSuccess = await _unitOfWork.SaveAsync() > 0;
        }, cancellationToken);
        return isSuccess;
    }
}
