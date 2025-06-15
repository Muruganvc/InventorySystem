using MediatR;
using Stock_Maintenance_System_Application.Product.CreateCommand;
using Stock_Maintenance_System_Domain.Product;
namespace Stock_Maintenance_System_Api.EndPoints;
public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/product", async (Product product, IMediator mediator) =>
        {
            var command = new ProductCreateCommand(
                product.ProductIdName,
                product.ProductCompany,
                product.ProductModel,
                product.Mrp,
                product.SalePrice,
                product.Quantity,
                product.TotalQuantity,
                product.PurchaseDate,
                product.IsActive,
                product.CreatedBy,
                product.CreatedDate,
                product.ModifiedBy,
                product.ModifiedDate
            );

            var resultId = await mediator.Send(command);

            return TypedResults.Ok(new
            {
                message = "Product created successfully",
                productId = resultId
            });
        });

        return app;
    }
}
