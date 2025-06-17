using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stock_Maintenance_System_Application.Company.Query;
using Stock_Maintenance_System_Application.ProductCompany.Query;

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


        app.MapGet("/product-company", async (
            [FromQuery] int companyId,
            IMediator mediator) =>
        {
            //if (string.IsNullOrWhiteSpace(companyName))
            //    return Results.BadRequest("companyName is required.");

            var query = new GetProductCompanyQuery(companyId);
            var result = await mediator.Send(query);
            return Results.Ok(new
            {
                message = "Company Product data",
                data = result
            });
        })
        .WithName("GetProductCompany")
        .WithTags("ProductCompany")
        .Produces<IEnumerable<GetProductCompanyQueryResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        //.RequireAuthorization()
        ;
        return app;
    }
}
