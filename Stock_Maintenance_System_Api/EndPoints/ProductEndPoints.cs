using MediatR;
using InventorySystem_Api.ApiRequest;
using InventorySystem_Application.Product.Command.ActivateProductCommand;
using InventorySystem_Application.Product.Command.CreateProductCommand;
using InventorySystem_Application.Product.Command.UpdateProductCommand;
using InventorySystem_Application.Product.Query.GetProducts;
using InventorySystem_Application.Product.Command.QuantityUpdateCommand;
using InventorySystem_Application.Company.Command.BulkCompanyCommand;
using InventorySystem_Application.ProductCategory.Query.GetCompanyCategoryProduct;
using InventorySystem_Application.Common;

namespace InventorySystem_Api.EndPoints;
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
                product.Mrp, product.SalesPrice, product.TotalQuantity, product.LandingPrice, product.IsActive);
            var result = await mediator.Send(command);
            return Results.Ok(result);
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
            return Results.Ok(result);
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
                product.SalesPrice, product.TotalQuantity, product.IsActive, product.LandingPrice);
            var result = await mediator.Send(command);
            return Results.Ok(result);
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
            return Results.Ok(result);
        })
        .WithName("ActivateProductCommand")
        .WithTags("UpdateProduct")
        .Produces<int>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization();

        app.MapPut("/product/{productId:int}/{quantity:int}", async (
        int productId,
        int quantity,
        IMediator mediator) =>
        {
            var command = new QuantityUpdateCommand(productId, quantity);
            var result = await mediator.Send(command);
            return Results.Ok(result);
        })
        .WithName("UpdateProductQuantity")
        .WithTags("Products")
        .Produces(StatusCodes.Status200OK, typeof(object))
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization();

        app.MapGet("/company-category-product", async ( IMediator mediator) =>
        {
            var command = new GetCompanyCategoryProductQuery();
            var result = await mediator.Send(command);
            return Results.Ok(result);
        })
        .WithName("GetCompanyCategoryProductQuery")
        .WithTags("CompanyCategoryProducts")
        .Produces(StatusCodes.Status200OK, typeof(IResult<IReadOnlyList<KeyValuePair<string, string>>>))
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization();
        return app;
    }
}
