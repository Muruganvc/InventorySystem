using MediatR;
using Stock_Maintenance_System_Api.ApiRequest;
using Stock_Maintenance_System_Application.Product.Command.ActivateProductCommand;
using Stock_Maintenance_System_Application.Product.Command.CreateProductCommand;
using Stock_Maintenance_System_Application.Product.Command.UpdateProductCommand;
using Stock_Maintenance_System_Application.Product.Query.GetProducts;

namespace Stock_Maintenance_System_Api.EndPoints;
public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/product", async (
            ProductRequest product,
            IMediator mediator) =>
        {
            var command = new CreateProductCommand(product.ProductName,
                product.CompanyId,
                product.CategoryId, product.ProductCategoryId, product.Description,
                product.Mrp, product.SalesPrice, product.TotalQuantity, product.IsActive, product.TaxType, product.BarCode,
                product.BrandName, product.TaxPercent
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
       .RequireAuthorization();
       

        app.MapGet("/products/{type}", async (string type, IMediator mediator) =>
        {
            var query = new GetProductsQuery(type);
            var result = await mediator.Send(query);
            return Results.Ok(new
            {
                message = "All Products",
                data = result
            });
        })
        .WithName("GetProductsQuery")
        .WithTags("Products")
        .Produces<IReadOnlyList<GetProductsQueryResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
       .RequireAuthorization();

        app.MapPut("/product/{productId}", async (int productId,
            UpdateProductRequest product,
            IMediator mediator) =>
        {
            var command = new UpdateProductCommand(productId, product.ProductName, product.CompanyId,
                product.CategoryId, product.ProductCategoryId, product.Description, product.Mrp,
                product.SalesPrice, product.TotalQuantity, product.IsActive, product.TaxType,
                product.BarCode, product.BarCode, product.TaxPercent);
            var result = await mediator.Send(command);
            return Results.Ok(new
            {
                message = "Product Update successfully",
                data = result
            });
        })
        .WithName("UpdateProductCommand")
        .WithTags("UpdateProduct")
        .Produces<int>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization();

        app.MapPut("/product/activate/{productId}", async (int productId,
            IMediator mediator) =>
        {
            var command = new ActivateProductCommand(productId);
            var result = await mediator.Send(command);
            return Results.Ok(new
            {
                message = "Product Update successfully",
                data = result
            });
        })
        .WithName("ActivateProductCommand")
        .WithTags("UpdateProduct")
        .Produces<int>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization();

        return app;
    }
}
