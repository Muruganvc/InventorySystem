using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stock_Maintenance_System_Api.ApiRequest;
using Stock_Maintenance_System_Application.Category.Command.CreateCommand;
using Stock_Maintenance_System_Application.Category.Command.UpdateCommand;
using Stock_Maintenance_System_Application.Category.Query.GetCategoryQuery;
using Stock_Maintenance_System_Application.Company.Command.CreateCommand;
using Stock_Maintenance_System_Application.Company.Command.UpdateCommand;
using Stock_Maintenance_System_Application.Company.Query;
using Stock_Maintenance_System_Application.ProductCategory.Command.CreateCommand;
using Stock_Maintenance_System_Application.ProductCategory.Command.UpdateCommand;
using Stock_Maintenance_System_Application.ProductCategory.Query.GetProductCategoryQuery;

namespace Stock_Maintenance_System_Api.EndPoints;

public static class CompanyEndPoints
{
    public static IEndpointRouteBuilder MapProductCompanyEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/company", async (
               CompanyCreateRequest company,
             IMediator mediator) =>
        {
            var query = new CompanyCreateCommand(company.CompanyName, company.Description, company.IsActive);
            var result = await mediator.Send(query);
            return Results.Ok(new
            {
                message = "Company Created.",
                data = result
            });
        })
         .WithName("CompanyCreateCommand")
         .WithTags("CompanyCreateRequest")
         .Produces<int>(StatusCodes.Status200OK)
         .Produces(StatusCodes.Status400BadRequest).
          RequireAuthorization();


        app.MapPut("/company/{companyId}", async (
            [FromQuery] int companyId,
               CompanyUpdateRequest company,
             IMediator mediator) =>
        {
            var query = new CompanyUpdateCommand(companyId, company.CompanyName, company.Description, company.IsActive);
            var result = await mediator.Send(query);
            return Results.Ok(new
            {
                message = "Company Updated.",
                data = result
            });
        })
         .WithName("CompanyUpdateCommand")
         .WithTags("CompanyUpdateRequest")
         .Produces<bool>(StatusCodes.Status200OK)
         .Produces(StatusCodes.Status400BadRequest).
          RequireAuthorization();

        app.MapPost("/category", async (
               CategoryCreateRequest category,
             IMediator mediator) =>
        {
            var query = new CategoryCreateCommand(category.CompanyId, category.CategoryName, category.Description, category.IsActive);
            var result = await mediator.Send(query);
            return Results.Ok(new
            {
                message = "Category Created.",
                data = result
            });
        })
         .WithName("CategoryCreateCommand")
         .WithTags("CategoryCreateRequest")
         .Produces<int>(StatusCodes.Status200OK)
         .Produces(StatusCodes.Status400BadRequest).
          RequireAuthorization();

        app.MapPut("/category/{categoryId}", async (
            [FromQuery] int categoryId,
               CategoryUpdateRequest category,
             IMediator mediator) =>
        {
            var query = new CategoryUpdateCommand(categoryId, category.CompanyId, category.CategoryName, category.Description, category.IsActive);
            var result = await mediator.Send(query);
            return Results.Ok(new
            {
                message = "Category Updated.",
                data = result
            });
        })
         .WithName("CategoryUpdateCommand")
         .WithTags("CategoryUpdateRequest")
         .Produces<bool>(StatusCodes.Status200OK)
         .Produces(StatusCodes.Status400BadRequest).
          RequireAuthorization();



        app.MapPost("/product-category", async (
               ProductCategoryCreateRequest productCategory,
             IMediator mediator) =>
        {
            var query = new ProductCategoryCreateCommand(productCategory.CategoryId,
                productCategory.CategoryProductName,productCategory.Description,productCategory.IsActive);
            var result = await mediator.Send(query);
            return Results.Ok(new
            {
                message = "Product Category Created.",
                data = result
            });
        })
         .WithName("ProductCategoryCreateCommand")
         .WithTags("ProductCategoryCreateRequest")
         .Produces<int>(StatusCodes.Status200OK)
         .Produces(StatusCodes.Status400BadRequest).
          RequireAuthorization();

        app.MapPut("/product-category/{productCategoryId}", async (
            [FromQuery] int productCategoryId,
               ProductCategoryUpdateRequest productCategory,
             IMediator mediator) =>
        {
            var query = new ProductCategoryUpdateCommand(productCategoryId, productCategory.CategoryId, productCategory.CategoryName,
                productCategory.Description, productCategory.IsActive);
            var result = await mediator.Send(query);
            return Results.Ok(new
            {
                message = "Product Category Updated.",
                data = result
            });
        })
         .WithName("ProductCategoryUpdateCommand")
         .WithTags("CompanyUpdateRequest")
         .Produces<bool>(StatusCodes.Status200OK)
         .Produces(StatusCodes.Status400BadRequest).
          RequireAuthorization();

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
        .Produces(StatusCodes.Status400BadRequest).
         RequireAuthorization();

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
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization();

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
           .Produces(StatusCodes.Status400BadRequest)
           .RequireAuthorization();

        return app;
    }
}