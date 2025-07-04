using InventorySystem_Domain;
using InventorySystem_Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem_Application.Dashboard.Query.TotalProductQuery;

internal class TotalProductQueryHandler : IRequestHandler<TotalProductQuery, TotalProductQueryResponse>
{
    private readonly IRepository<OrderItem> _orderItemRepository;
    private readonly IRepository<InventorySystem_Domain.Order> _orderRepository;
    private readonly IRepository<InventorySystem_Domain.Product> _productRepository;
    public TotalProductQueryHandler(IRepository<OrderItem> orderItemRepository, IRepository<InventorySystem_Domain.Order> orderRepository, IRepository<InventorySystem_Domain.Product> productRepository)
    {
        _orderItemRepository = orderItemRepository;
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }
    public async Task<TotalProductQueryResponse> Handle(TotalProductQuery request, CancellationToken cancellationToken)
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

        return new TotalProductQueryResponse(totalQuantity, totalNetTotal,100);
    }


}
