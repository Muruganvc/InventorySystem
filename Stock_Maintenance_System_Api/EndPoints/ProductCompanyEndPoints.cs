using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stock_Maintenance_System_Application.Category.Query.GetCategoryQuery;
using Stock_Maintenance_System_Application.Company.Query;
using Stock_Maintenance_System_Application.ProductCategory.Query.GetProductCategoryQuery; 

namespace Stock_Maintenance_System_Api.EndPoints;

public static class ProductCompanyEndPoints
{
    public static IEndpointRouteBuilder MapProductCompanyEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/company", async (
            [FromQuery] string? companyName,
            IMediator mediator) =>
        {
            var query = new GetCompanyQuery(companyName);
            var result = await mediator.Send(query);
            return Results.Ok(new
            {
                message = "Company Product data",
                data = result
            });
        })
        .WithName("GetCompanyQuery")
        .WithTags("Company")
        .Produces<IReadOnlyList<KeyValuePair<string, int>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
        // .RequireAuthorization(); // Uncomment if auth is required


        app.MapGet("/category/{companyId}", async (int companyId, IMediator mediator) =>
        {
            var query = new GetCategoryQuery(companyId);
            var result = await mediator.Send(query);
            return Results.Ok(new
            {
                message = "Category Product data",
                data = result
            });
        })
        .WithName("GetCategoryQuery")
        .WithTags("Category")
        .Produces<IReadOnlyList<KeyValuePair<string, int>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
        // .RequireAuthorization(); // Uncomment if auth is required


        app.MapGet("/product-category/{categoryId}", async (int categoryId, IMediator mediator) =>
        {
            var query = new GetProductCategoryQuery(categoryId);
            var result = await mediator.Send(query);
            return Results.Ok(new
            {
                message = "Product Category Product data",
                data = result
            });
        })
           .WithName("GetProductCategoryQuery")
           .WithTags("ProductCategoryQuery")
           .Produces<IReadOnlyList<KeyValuePair<string, int>>>(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status400BadRequest);
                // .RequireAuthorization(); // Uncomment if auth is required
        
        return app;
    }
}
