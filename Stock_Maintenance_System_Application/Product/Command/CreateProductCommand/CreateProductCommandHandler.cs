using MediatR;
using Stock_Maintenance_System_Domain.Common;

namespace Stock_Maintenance_System_Application.Product.Command.CreateProductCommand;
internal sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Stock_Maintenance_System_Domain.Product> _productRepository;
    public CreateProductCommandHandler(IUnitOfWork unitOfWork,
        IRepository<Stock_Maintenance_System_Domain.Product> productRepository)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
    }
    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Stock_Maintenance_System_Domain.Product
        {
            Barcode = request.BarCode,
            BrandName = request.BrandName,
            CategoryId = request.CategoryId,
            CompanyId = request.CompanyId,
            ProductCategoryId = request.ProductCategoryId,
            CreatedAt = DateTime.Now,
            Description = request.Description,
            IsActive = false,
            SalesPrice = request.SalesPrice,
            MRP = request.Mrp,
            ProductName = request.ProductName,
            Quantity = request.TotalQuantity,
            TaxType = request.TaxType,
            TaxPercent = request.TaxPercent,
            CreatedBy = 1
        };

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
       {
           await _unitOfWork.Repository<Stock_Maintenance_System_Domain.Product>().AddAsync(product);
           await _unitOfWork.SaveAsync();
       });
        return product.ProductId;
    }
}
