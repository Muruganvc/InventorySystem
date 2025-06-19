using MediatR;
using Stock_Maintenance_System_Api.ApiRequest;
using Stock_Maintenance_System_Application.Product.Command.CreateProductCommand;
using Stock_Maintenance_System_Application.Product.Query.GetProducts;

namespace Stock_Maintenance_System_Api.EndPoints;
public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/product-company", async (
            ProductRequest product,
            IMediator mediator) =>
        {
            var command = new CreateProductCommand(product.ProductName,
                product.CompanyId,
                product.CategoryId, product.Description,
                product.Mrp, product.SalesPrice,product.TotalQuantity
                );
            var result = await mediator.Send(command);
            return Results.Ok(new
            {
                message = "Product created successfully",
                data = result
            });
        })
        .WithName("CreateProductCommand")
        .WithTags("CreateProduct")
        .Produces<int>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        //.RequireAuthorization()
        ;

        app.MapGet("/products", async (IMediator mediator) =>
        {
            var query = new GetProductsQuery();
            var result = await mediator.Send(query);
            return Results.Ok(new
            {
                message = "All Products",
                data = result
            });
        })
        .WithName("GetProductsTypeQuery")
        .WithTags("Products")
        .Produces<IReadOnlyList<GetProductsQueryResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
        // .RequireAuthorization(); // Uncomment if auth is required

        return app;
    }
}
