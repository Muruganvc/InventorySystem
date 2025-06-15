using MediatR;
using Stock_Maintenance_System_Domain.Common;
namespace Stock_Maintenance_System_Application.Product.CreateCommand;
internal sealed class ProductCreateCommandHandler : IRequestHandler<ProductCreateCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    public ProductCreateCommandHandler(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;
    public async Task<int> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
    {
        var product = new Stock_Maintenance_System_Domain.Product.Product
        {
            ProductId = 0,
            CreatedBy = request.CreatedBy,
            CreatedDate = DateTime.Now,
            IsActive = request.IsActive,
            ModifiedBy = request.ModifiedBy,
            ModifiedDate = DateTime.Now,
            Mrp = request.Mrp,
            SalePrice = request.SalePrice,
            ProductCompany = request.ProductCompany,
            ProductModel = request.ProductModel,
            ProductIdName = request.ProductIdName,
            PurchaseDate = request.PurchaseDate,
            Quantity = request.Quantity,
            TotalQuantity = request.TotalQuantity,
        };
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            await _unitOfWork.Repository<Stock_Maintenance_System_Domain.Product.Product>().AddAsync(product);
            await _unitOfWork.SaveAsync();
        });
        return product.ProductId;
    }
}