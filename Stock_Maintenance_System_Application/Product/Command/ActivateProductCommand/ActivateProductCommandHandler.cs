using InventorySystem_Application.Common;
using InventorySystem_Domain.Common;
using MediatR;

namespace InventorySystem_Application.Product.Command.ActivateProductCommand;

internal sealed class ActivateProductCommandHandler : IRequestHandler<ActivateProductCommand, IResult<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<InventorySystem_Domain.Product> _productRepository;
    private readonly IUserInfo _userInfo;
    public ActivateProductCommandHandler(IUnitOfWork unitOfWork,
        IRepository<InventorySystem_Domain.Product> productRepository, IUserInfo userInfo)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _userInfo = userInfo;
    }
    public async Task<IResult<bool>> Handle(ActivateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByAsync(a => a.ProductId == request.ProductId);
        if (product == null)
            return Result<bool>.Failure("Selected product not found.");
        product.IsActive = !product.IsActive;
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
