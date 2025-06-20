using MediatR;
using Stock_Maintenance_System_Domain.Common;

namespace Stock_Maintenance_System_Application.Product.Command.UpdateProductCommand;
internal sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Stock_Maintenance_System_Domain.Product> _productRepository;
    public UpdateProductCommandHandler(IUnitOfWork unitOfWork,
        IRepository<Stock_Maintenance_System_Domain.Product> productRepository)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
    }
    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
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
        product.UpdatedBy = 1;
        product.UpdatedAt= DateTime.Now;
        product.Description= request.Description;
        product.IsActive= request.IsActive;
        bool isSuccess = false;
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            isSuccess = await _unitOfWork.SaveAsync() > 0;
        });
        return isSuccess;
    }
}
