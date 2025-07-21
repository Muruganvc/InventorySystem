using InventorySystem_Application.Dashboard.Query.AuditQuery;
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
                return Results.Ok(result);
            })
          .RequireAuthorization("AllRoles");

            app.MapGet("/product-sold", async (IMediator mediator) =>
            {
                var query = new TotalProductQuery();
                var result = await mediator.Send(query);
                return Results.Ok(result);
            })
          .RequireAuthorization("AllRoles");

            app.MapGet("/product-quantity", async (IMediator mediator) =>
            {
                var query = new ProductQuantityQuery();
                var result = await mediator.Send(query);
                return Results.Ok(result);
            })
           .RequireAuthorization("AllRoles");

            app.MapGet("/audit-log", async (IMediator mediator) =>
            {
                var query = new AuditQuery();
                var result = await mediator.Send(query);
                return Results.Ok(result);
            })
       .RequireAuthorization("AllRoles");


            return app;
        }
    }
}
