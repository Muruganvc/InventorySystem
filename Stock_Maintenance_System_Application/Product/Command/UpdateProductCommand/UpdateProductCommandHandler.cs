using InventorySystem_Application.Common;
using InventorySystem_Domain.Common;
using MediatR;

namespace InventorySystem_Application.Product.Command.UpdateProductCommand;
internal sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, IResult<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<InventorySystem_Domain.Product> _productRepository;
    private readonly IUserInfo _userInfo;
    public UpdateProductCommandHandler(IUnitOfWork unitOfWork,
        IRepository<InventorySystem_Domain.Product> productRepository,  IUserInfo userInfo)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _userInfo = userInfo;
    }
    public async Task<IResult<bool>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByAsync(a => a.ProductId == request.ProductId);
        if (product == null)
            return Result<bool>.Failure("Selected product not found");

        product.Description = request.Description;
        product.CategoryId = request.CategoryId;
        product.ProductCategoryId = request.ProductCategoryId;
        product.ProductName = request.ProductName;
        product.MRP = request.Mrp;
        product.SalesPrice = request.SalesPrice;
        product.Quantity = request.TotalQuantity;
        product.LandingPrice = request.LandingPrice;
        product.CompanyId = request.CompanyId;
        product.UpdatedBy = _userInfo.UserId;
        product.UpdatedAt = DateTime.Now;
        product.Description = request.Description;
        product.IsActive = request.IsActive; 
        bool isSuccess = false;
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            isSuccess = await _unitOfWork.SaveAsync() > 0;
        }, cancellationToken);
        return Result<bool>.Success(isSuccess);
    }
}
