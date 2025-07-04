using InventorySystem_Application.Dashboard.Query.CompanyWiseIncomeQuery;
using InventorySystem_Application.Dashboard.Query.ProductQuantityQuery;
using InventorySystem_Application.Dashboard.Query.TotalProductQuery;
using MediatR;

namespace InventorySystem_Api.EndPoints
{
    public static class DashboradEndPoints
    {
        public static IEndpointRouteBuilder MapDashboardEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/company-wise-income", async (IMediator mediator) =>
            {
                var query = new CompanyWiseIncomeQuery();
                var result = await mediator.Send(query);
                return Results.Ok(new { message = "Company Wise Income", data = result });
            })
           .RequireAuthorization();

            app.MapGet("/product-sold", async (IMediator mediator) =>
            {
                var query = new TotalProductQuery();
                var result = await mediator.Send(query);
                return Results.Ok(new { message = "Total Product", data = result });
            })
           .RequireAuthorization();

            app.MapGet("/product-quantity", async (IMediator mediator) =>
            {
                var query = new ProductQuantityQuery();
                var result = await mediator.Send(query);
                return Results.Ok(new { message = "Total Product", data = result });
            })
            .RequireAuthorization();

            return app;
        }
    }
}
