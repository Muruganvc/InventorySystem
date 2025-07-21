using InventorySystem_Api.ApiRequest;
using InventorySystem_Application.Order.Command.OrderCreateCommand;
using InventorySystem_Application.Order.Query;
using InventorySystem_Application.Order.Query.GetCustomerOrderSummary;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem_Api.EndPoints;

public static class OrderEndPoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/new-order", async (
            [FromBody] OrderCreateRequest order,
            IMediator mediator) =>
        {
            var customerCommand = new CustomerCommand(
                order.Customer.CustomerId,
                order.Customer.CustomerName,
                order.Customer.Phone,
                order.Customer.Address
            );

            var orderItemCommands = order.OrderItemRequests.Select(item => new OrderItemCommand(
                item.ProductId,
                item.Quantity,
                item.UnitPrice,
                item.DiscountPercent,
                item.SerialNo
            )).ToList();

            var command = new OrderCreateCommand(customerCommand, orderItemCommands, order.GivenAmount, order.IsGst, order.GstNumber);
            var result = await mediator.Send(command);
            return Results.Ok(result);
        })
        .WithName("CreateOrder")
        .WithTags("Order")
        .Produces<int>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization("AllRoles");

        app.MapGet("/order-summary", async ([FromQuery] int OrderId,
            IMediator mediator) =>
        {
            var query = new GetOrdersummaryQuery(OrderId);
            var result = await mediator.Send(query);
            return Results.Ok(result);
        })
        .WithName("GetOrdersummaryQuery")
        .WithTags("OrderList")
        .Produces<IReadOnlyList<GetOrderSummaryResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization("AllRoles");

        app.MapGet("/customer-orders", async (
            IMediator mediator) =>
        {
            var query = new GetCustomerOrderSummaryQuery();
            var result = await mediator.Send(query);
            return Results.Ok(result);
        })
        .WithName("GetCustomerOrderSummaryQuery")
        .WithTags("OrderList")
        .Produces<IReadOnlyList<GetCustomerOrderSummaryResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization("AllRoles");

        return app;
    }
}
