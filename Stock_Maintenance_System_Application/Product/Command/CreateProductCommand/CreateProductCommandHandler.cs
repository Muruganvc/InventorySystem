using InventorySystem_Application.Common;
using InventorySystem_Domain.Common;
using MediatR;

namespace InventorySystem_Application.Product.Command.CreateProductCommand;
internal sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, IResult<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<InventorySystem_Domain.Product> _productRepository;
    private readonly IUserInfo _userInfo;
    public CreateProductCommandHandler(IUnitOfWork unitOfWork,
        IRepository<InventorySystem_Domain.Product> productRepository,
        IUserInfo userInfo)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _userInfo = userInfo;
    }
    public async Task<IResult<int>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // Determine product category ID (create new if needed)
        int? productCategoryId = request.ProductCategoryId;

        var isExistProduct = await _productRepository.GetByAsync(a => a.ProductCategoryId == request.ProductCategoryId && a.CategoryId == request.CategoryId && a.CompanyId == request.CompanyId);
        if (isExistProduct is not null)
            return Result<int>.Failure("Selected product already exists.");

        if (productCategoryId is null or 0)
        {
            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var newProductCategory = new InventorySystem_Domain.ProductCategory
                {
                    CategoryId = request.CategoryId,
                    ProductCategoryName = request.ProductName,
                    CreatedBy = _userInfo.UserId
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
            CreatedBy = _userInfo.UserId
        };

        // Save the product
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            await _unitOfWork.Repository<InventorySystem_Domain.Product>().AddAsync(product);
            await _unitOfWork.SaveAsync();
        }, cancellationToken);
        return Result<int>.Success(product.ProductId);
    }
}
