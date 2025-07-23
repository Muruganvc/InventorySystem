using InventorySystem_Application.Dashboard.Query.AuditQuery;
using InventorySystem_Application.Dashboard.Query.CompanyWiseIncomeQuery;
using InventorySystem_Application.Dashboard.Query.IncomeOrOutcomeSummaryReportQuery;
using InventorySystem_Application.Dashboard.Query.ProductQuantityQuery;
using InventorySystem_Application.Dashboard.Query.TotalProductQuery;
using MediatR;

namespace InventorySystem_Api.EndPoints;

public static class DashboardEndpoints
{
    public static IEndpointRouteBuilder MapDashboardEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/company-wise-income", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new CompanyWiseIncomeQuery());
            return Results.Ok(result);
        }).RequireAuthorization("AllRoles");

        app.MapGet("/product-sold", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new TotalProductQuery());
            return Results.Ok(result);
        }).RequireAuthorization("AllRoles");

        app.MapGet("/product-quantity", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new ProductQuantityQuery());
            return Results.Ok(result);
        }).RequireAuthorization("AllRoles");

        app.MapGet("/audit-log", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new AuditQuery());
            return Results.Ok(result);
        }).RequireAuthorization("AllRoles");

        app.MapGet("/income-outcome-summary-report", async (
            DateTime? fromDate,
            DateTime? endDate,
            IMediator mediator) =>
        {
            var result = await mediator.Send(new IncomeOrOutcomeSummaryReportQuery(fromDate, endDate));
            return Results.Ok(result);
        }).RequireAuthorization("AdminOnly");

        return app;
    }
}
