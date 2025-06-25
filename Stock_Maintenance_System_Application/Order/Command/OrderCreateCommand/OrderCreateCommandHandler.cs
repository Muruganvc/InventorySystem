using MediatR;
using Stock_Maintenance_System_Domain;
using Stock_Maintenance_System_Domain.Common;

namespace Stock_Maintenance_System_Application.Order.Command.OrderCreateCommand;

internal sealed class OrderCreateCommandHandler : IRequestHandler<OrderCreateCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderCreateCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<int> Handle(OrderCreateCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            // 1. Handle customer
            int customerId = request.Customer.CustomerId;
            if (customerId == 0)
            {
                var newCustomer = new Stock_Maintenance_System_Domain.Customer
                {
                    CustomerName = request.Customer.CustomerName,
                    Address = request.Customer.Address,
                    Phone = request.Customer.Phone,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Repository<Stock_Maintenance_System_Domain.Customer>().AddAsync(newCustomer);
                await _unitOfWork.SaveAsync();
                customerId = newCustomer.CustomerId;
            }

            // 2. Create initial order
            var newOrder = new Stock_Maintenance_System_Domain.Order
            {
                CustomerId = customerId,
                OrderDate = DateTime.UtcNow,
                BalanceAmount = 0,
                FinalAmount = 0,
                TotalAmount = 0
            };

            await _unitOfWork.Repository<Stock_Maintenance_System_Domain.Order>().AddAsync(newOrder);
            await _unitOfWork.SaveAsync();
            // 3. Add Order Items
            var orderItems = request.OrderItemRequests.Select(item => new OrderItem
            {
                OrderId = newOrder.OrderId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                DiscountPercent = item.DiscountPercent,
                Remarks = item.Remarks,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = 1 // You can later replace this with actual user ID
            }).ToList();

            orderItems.ForEach(async a =>
            {
                await _unitOfWork.Repository<OrderItem>().AddAsync(a);
            });
            await _unitOfWork.SaveAsync();
            // 4. Calculate totals
            var totalAmount = orderItems.Sum(i => i.Quantity * i.UnitPrice);
            var finalAmount = orderItems.Sum(i => i.Quantity * i.UnitPrice * (1 - i.DiscountPercent / 100.0m));

            // 5. Update order totals
            newOrder.TotalAmount = totalAmount;
            newOrder.FinalAmount = finalAmount;
            newOrder.BalanceAmount = finalAmount;

            // No need to call Update() explicitly if tracked by EF Core
            await _unitOfWork.SaveAsync(); // Single save for all changes
        }, cancellationToken);

        return 1; // Or return newOrder.OrderId if needed
    }
}
