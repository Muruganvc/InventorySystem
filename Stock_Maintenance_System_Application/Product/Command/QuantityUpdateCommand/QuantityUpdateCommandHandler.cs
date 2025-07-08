using InventorySystem_Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace InventorySystem_Application.Product.Command.QuantityUpdateCommand;
internal sealed class QuantityUpdateCommandHandler : IRequestHandler<QuantityUpdateCommand, bool>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<InventorySystem_Domain.Product> _productRepository;
    public QuantityUpdateCommandHandler(IUnitOfWork unitOfWork,
        IRepository<InventorySystem_Domain.Product> productRepository, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<bool> Handle(QuantityUpdateCommand request, CancellationToken cancellationToken)
    {
        int userId = int.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
        var product = await _productRepository.GetByAsync(a => a.ProductId == request.ProductId);
        if (product == null) return false;
        product.Quantity = request.Quantity;
        product.UpdatedBy = userId;
        product.UpdatedAt = DateTime.Now;
        bool isSuccess = false;
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            isSuccess = await _unitOfWork.SaveAsync() > 0;
        }, cancellationToken);
        return isSuccess;
    }
}
