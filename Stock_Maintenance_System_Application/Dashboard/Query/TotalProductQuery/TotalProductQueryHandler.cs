using InventorySystem_Application.Common;
using InventorySystem_Domain;
using InventorySystem_Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem_Application.Dashboard.Query.TotalProductQuery;

internal class TotalProductQueryHandler : IRequestHandler<TotalProductQuery, IResult<TotalProductQueryResponse>>
{
    private readonly IRepository<OrderItem> _orderItemRepository;
    private readonly IRepository<InventorySystem_Domain.Product> _productRepository;
    public TotalProductQueryHandler(IRepository<OrderItem> orderItemRepository, IRepository<InventorySystem_Domain.Product> productRepository)
    {
        _orderItemRepository = orderItemRepository;
        _productRepository = productRepository;
    }
    public async Task<IResult<TotalProductQueryResponse>> Handle(TotalProductQuery request, CancellationToken cancellationToken)
    {
        var result = await _orderItemRepository.Table
            .Join(_productRepository.Table,
                  orderItem => orderItem.ProductId,
                  product => product.ProductId,
                  (orderItem, product) => new
                  {
                      orderItem.Quantity,
                      NetTotal = (product.MRP - orderItem.UnitPrice) * orderItem.Quantity
                  })
            .ToListAsync(cancellationToken);

        var totalQuantity = result.Sum(x => x.Quantity);
        var totalNetTotal = result.Sum(x => x.NetTotal);

        return Result<TotalProductQueryResponse>.Success(new TotalProductQueryResponse(totalQuantity, totalNetTotal, 100));
    }
}
