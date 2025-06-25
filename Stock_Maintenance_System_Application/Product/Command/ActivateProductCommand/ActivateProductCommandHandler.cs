using MediatR;
using Stock_Maintenance_System_Domain.Common;

namespace Stock_Maintenance_System_Application.Product.Command.ActivateProductCommand;

internal sealed class ActivateProductCommandHandler : IRequestHandler<ActivateProductCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Stock_Maintenance_System_Domain.Product> _productRepository;
    public ActivateProductCommandHandler(IUnitOfWork unitOfWork,
        IRepository<Stock_Maintenance_System_Domain.Product> productRepository)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
    }
    public async Task<bool> Handle(ActivateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByAsync(a => a.ProductId == request.ProductId);
        if (product == null) return false;
        product.IsActive= !product.IsActive;
        bool isSuccess = false;
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            isSuccess = await _unitOfWork.SaveAsync() > 0;
        }, cancellationToken);
        return isSuccess;
    }
}
