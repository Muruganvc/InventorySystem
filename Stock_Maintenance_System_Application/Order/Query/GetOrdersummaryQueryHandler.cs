using MediatR;
using Microsoft.EntityFrameworkCore;
using InventorySystem_Domain.Common;
using InventorySystem_Application.Common;

namespace InventorySystem_Application.Order.Query;

internal class GetOrdersummaryQueryHandler
    : IRequestHandler<GetOrdersummaryQuery, IResult<IReadOnlyList<GetOrderSummaryResponse>>>
{
    private readonly IRepository<InventorySystem_Domain.Category> _categoryRepository;
    private readonly IRepository<InventorySystem_Domain.Product> _productRepository;
    private readonly IRepository<InventorySystem_Domain.OrderItem> _orderItemRepository;
    private readonly IRepository<InventorySystem_Domain.Order> _orderRepository;
    private readonly IRepository<InventorySystem_Domain.Customer> _customerRepository;
    private readonly IRepository<InventorySystem_Domain.Company> _companyRepository;

    public GetOrdersummaryQueryHandler(
        IRepository<InventorySystem_Domain.Company> companyRepository,
        IRepository<InventorySystem_Domain.Category> categoryRepository,
        IRepository<InventorySystem_Domain.Product> productRepository,
        IRepository<InventorySystem_Domain.OrderItem> orderItemRepository,
        IRepository<InventorySystem_Domain.Order> orderRepository,
        IRepository<InventorySystem_Domain.Customer> customerRepository
    )
    {
        _companyRepository = companyRepository;
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _orderItemRepository = orderItemRepository;
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
    }

    public async Task<IResult<IReadOnlyList<GetOrderSummaryResponse>>> Handle(
        GetOrdersummaryQuery request,
        CancellationToken cancellationToken)
    {
        var resultList = await _orderItemRepository.Table
    .Join(_productRepository.Table,
        odIm => odIm.ProductId,
        pro => pro.ProductId,
        (odIm, pro) => new { odIm, pro })
    .Join(_categoryRepository.Table,
        temp => temp.pro.CategoryId,
        cat => cat.CategoryId,
        (temp, cat) => new { temp.odIm, temp.pro, cat })
    .Join(_companyRepository.Table,
        temp => temp.cat.CompanyId,
        com => com.CompanyId,
        (temp, com) => new { temp.odIm, temp.pro, temp.cat, com })
    .Join(_orderRepository.Table.Where(ord => ord.OrderId == request.OrderId),
        temp => temp.odIm.OrderId,
        odr => odr.OrderId,
        (temp, odr) => new { temp.odIm, temp.pro, temp.cat, temp.com, odr })
    .Join(_customerRepository.Table,
        temp => temp.odr.CustomerId,
        cus => cus.CustomerId,
        (temp, cus) => new GetOrderSummaryResponse
        {
            ProductId = temp.odIm.ProductId,
            FullProductName = $"{temp.com.CompanyName} {temp.cat.CategoryName} {temp.pro.ProductName}",
            Quantity = temp.odIm.Quantity,
            UnitPrice = temp.odIm.UnitPrice,
            DiscountPercent = temp.odIm.DiscountPercent,
            DiscountAmount = temp.odIm.DiscountAmount,
            SubTotal = temp.odIm.SubTotal,
            NetTotal = temp.odIm.NetTotal,
            OrderDate = temp.odr.OrderDate,
            FinalAmount = temp.odr.FinalAmount ?? 0,
            TotalAmount = temp.odr.TotalAmount ?? 0,
            BalanceAmount = temp.odr.BalanceAmount ?? 0,
            CustomerName = cus.CustomerName,
            Address = cus.Address,
            Phone = cus.Phone,
            User = temp.odIm.CreatedByUser.Username,
            IsGst = temp.odr.IsGst
        })
    .ToListAsync(cancellationToken);
        return Result<IReadOnlyList<GetOrderSummaryResponse>>.Success(resultList.OrderBy(a => a.FullProductName).ToList());
    }
}
