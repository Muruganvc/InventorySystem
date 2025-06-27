using MediatR;
using Microsoft.AspNetCore.Http;
using Stock_Maintenance_System_Domain.Common;
using System.Security.Claims;

namespace Stock_Maintenance_System_Application.Product.Command.ActivateProductCommand;

internal sealed class ActivateProductCommandHandler : IRequestHandler<ActivateProductCommand, bool>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Stock_Maintenance_System_Domain.Product> _productRepository;
    public ActivateProductCommandHandler(IUnitOfWork unitOfWork,
        IRepository<Stock_Maintenance_System_Domain.Product> productRepository, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<bool> Handle(ActivateProductCommand request, CancellationToken cancellationToken)
    {
        int userId = int.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
        var product = await _productRepository.GetByAsync(a => a.ProductId == request.ProductId);
        if (product == null) return false;
        product.IsActive= !product.IsActive;
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
