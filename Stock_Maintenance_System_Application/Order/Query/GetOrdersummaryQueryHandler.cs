using MediatR;
using Microsoft.EntityFrameworkCore;
using Stock_Maintenance_System_Domain.Common;

namespace Stock_Maintenance_System_Application.Order.Query;

internal class GetOrdersummaryQueryHandler
    : IRequestHandler<GetOrdersummaryQuery, IReadOnlyList<GetOrderSummaryResponse>>
{
    private readonly IRepository<Stock_Maintenance_System_Domain.Category> _categoryRepository;
    private readonly IRepository<Stock_Maintenance_System_Domain.Product> _productRepository;
    private readonly IRepository<Stock_Maintenance_System_Domain.OrderItem> _orderItemRepository;
    private readonly IRepository<Stock_Maintenance_System_Domain.Order> _orderRepository;
    private readonly IRepository<Stock_Maintenance_System_Domain.Customer> _customerRepository;
    private readonly IRepository<Stock_Maintenance_System_Domain.Company> _companyRepository;

    public GetOrdersummaryQueryHandler(
        IRepository<Stock_Maintenance_System_Domain.Company> companyRepository,
        IRepository<Stock_Maintenance_System_Domain.Category> categoryRepository,
        IRepository<Stock_Maintenance_System_Domain.Product> productRepository,
        IRepository<Stock_Maintenance_System_Domain.OrderItem> orderItemRepository,
        IRepository<Stock_Maintenance_System_Domain.Order> orderRepository,
        IRepository<Stock_Maintenance_System_Domain.Customer> customerRepository
    )
    {
        _companyRepository = companyRepository;
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _orderItemRepository = orderItemRepository;
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
    }

    public async Task<IReadOnlyList<GetOrderSummaryResponse>> Handle(
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
            Phone = cus.Phone
        })
    .ToListAsync(cancellationToken);

        return resultList;
    }
}
