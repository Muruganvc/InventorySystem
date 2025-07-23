using InventorySystem_Application.Common;
using InventorySystem_Domain.Common;
using MediatR;

namespace InventorySystem_Application.Product.Command.QuantityUpdateCommand;
internal sealed class QuantityUpdateCommandHandler : IRequestHandler<QuantityUpdateCommand, IResult<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<InventorySystem_Domain.Product> _productRepository;
    private readonly IUserInfo _userInfo;
    public QuantityUpdateCommandHandler(IUnitOfWork unitOfWork,
        IRepository<InventorySystem_Domain.Product> productRepository, IUserInfo userInfo)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _userInfo = userInfo;
    }
    public async Task<IResult<bool>> Handle(QuantityUpdateCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByAsync(a => a.ProductId == request.ProductId);
        if (product == null)
            return Result<bool>.Failure("Selected product not found"); 

        product.Quantity = request.Quantity;
        product.UpdatedBy = _userInfo.UserId;
        product.UpdatedAt = DateTime.Now;
        bool isSuccess = false;
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            isSuccess = await _unitOfWork.SaveAsync() > 0;
        }, cancellationToken);
        return Result<bool>.Success(isSuccess);
    }
}
