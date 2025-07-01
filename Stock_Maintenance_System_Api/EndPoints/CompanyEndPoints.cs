using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stock_Maintenance_System_Api.ApiRequest;
using Stock_Maintenance_System_Application.Category.Command.CreateCommand;
using Stock_Maintenance_System_Application.Category.Command.UpdateCommand;
using Stock_Maintenance_System_Application.Category.Query.GetCategoriesQuery;
using Stock_Maintenance_System_Application.Category.Query.GetCategoryQuery;
using Stock_Maintenance_System_Application.Company.Command.CreateCommand;
using Stock_Maintenance_System_Application.Company.Command.UpdateCommand;
using Stock_Maintenance_System_Application.Company.Query;
using Stock_Maintenance_System_Application.ProductCategory.Command.CreateCommand;
using Stock_Maintenance_System_Application.ProductCategory.Command.UpdateCommand;
using Stock_Maintenance_System_Application.ProductCategory.Query.GetProductCategoriesQuery;
using Stock_Maintenance_System_Application.ProductCategory.Query.GetProductCategoryQuery;

namespace Stock_Maintenance_System_Api.EndPoints;

public static class CompanyEndPoints
{
    public static IEndpointRouteBuilder MapProductCompanyEndpoints(this IEndpointRouteBuilder app)
    {
        #region Company

        app.MapPost("/company", async ([FromBody] CompanyCreateRequest company, IMediator mediator) =>
        {
            var command = new CompanyCreateCommand(company.CompanyName, company.Description, company.IsActive);
            var result = await mediator.Send(command);
            return Results.Ok(new { message = "Company Created.", data = result });
        })
        .WithName("CreateCompany")
        .WithTags("Company")
        .Produces<int>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization();

        app.MapPut("/company/{companyId:int}", async (int companyId, [FromBody] CompanyUpdateRequest company, IMediator mediator) =>
        {
            var command = new CompanyUpdateCommand(companyId, company.CompanyName, company.Description, company.IsActive);
            var result = await mediator.Send(command);
            return Results.Ok(new { message = "Company Updated.", data = result });
        })
        .WithName("UpdateCompany")
        .WithTags("Company")
        .Produces<bool>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization();

        app.MapGet("/company", async ([FromQuery] string? companyName, IMediator mediator) =>
        {
            var query = new GetCompanyQuery(companyName);
            var result = await mediator.Send(query);
            return Results.Ok(new { message = "Company Product data", data = result });
        })
        .WithName("GetCompanyList")
        .WithTags("Company")
        .Produces<IReadOnlyList<KeyValuePair<string, int>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization();

        #endregion

        #region Category

        app.MapPost("/category", async ([FromBody] CategoryCreateRequest category, IMediator mediator) =>
        {
            var command = new CategoryCreateCommand(category.CompanyId, category.CategoryName, category.Description, category.IsActive);
            var result = await mediator.Send(command);
            return Results.Ok(new { message = "Category Created.", data = result });
        })
        .WithName("CreateCategory")
        .WithTags("Category")
        .Produces<int>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization();

        app.MapPut("/category/{categoryId:int}", async (int categoryId, [FromBody] CategoryUpdateRequest category, IMediator mediator) =>
        {
            var command = new CategoryUpdateCommand(categoryId, category.CompanyId, category.CategoryName, category.Description, category.IsActive);
            var result = await mediator.Send(command);
            return Results.Ok(new { message = "Category Updated.", data = result });
        })
        .WithName("UpdateCategory")
        .WithTags("Category")
        .Produces<bool>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization();

        app.MapGet("/category/{companyId:int}", async (int companyId, IMediator mediator) =>
        {
            var query = new GetCategoryQuery(companyId);
            var result = await mediator.Send(query);
            return Results.Ok(new { message = "Category Product data", data = result });
        })
        .WithName("GetCategoriesByCompany")
        .WithTags("Category")
        .Produces<IReadOnlyList<KeyValuePair<string, int>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization();

        app.MapGet("/categories", async (IMediator mediator) =>
        {
            var query = new GetCategoriesQuery();
            var result = await mediator.Send(query);
            return Results.Ok(new { message = "All Categories", data = result });
        })
        .WithName("GetAllCategories")
        .WithTags("Category")
        .Produces<IReadOnlyList<GetCategoryQueryResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization();

        #endregion

        #region Product Category

        app.MapPost("/product-category", async ([FromBody] ProductCategoryCreateRequest productCategory, IMediator mediator) =>
        {
            var command = new ProductCategoryCreateCommand(
                productCategory.CategoryId,
                productCategory.CategoryProductName,
                productCategory.Description,
                productCategory.IsActive);
            var result = await mediator.Send(command);
            return Results.Ok(new { message = "Product Category Created.", data = result });
        })
        .WithName("CreateProductCategory")
        .WithTags("ProductCategory")
        .Produces<int>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization();

        app.MapPut("/product-category/{productCategoryId:int}", async (int productCategoryId, [FromBody] ProductCategoryUpdateRequest productCategory, IMediator mediator) =>
        {
            var command = new ProductCategoryUpdateCommand(
                productCategoryId,
                productCategory.CategoryId,
                productCategory.ProductCategoryName,
                productCategory.Description,
                productCategory.IsActive);
            var result = await mediator.Send(command);
            return Results.Ok(new { message = "Product Category Updated.", data = result });
        })
        .WithName("UpdateProductCategory")
        .WithTags("ProductCategory")
        .Produces<bool>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization();

        app.MapGet("/product-category/{categoryId:int}", async (int categoryId, IMediator mediator) =>
        {
            var query = new GetProductCategoryQuery(categoryId);
            var result = await mediator.Send(query);
            return Results.Ok(new { message = "Product Category data", data = result });
        })
        .WithName("GetProductCategoriesByCategory")
        .WithTags("ProductCategory")
        .Produces<IReadOnlyList<KeyValuePair<string, int>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization();

        app.MapGet("/product-categories", async (IMediator mediator) =>
        {
            var query = new GetProductCategoriesQuery();
            var result = await mediator.Send(query);
            return Results.Ok(new { message = "Product Categories data", data = result });
        })
        .WithName("GetProductCategoriesQuery")
        .WithTags("ProductCategory")
        .Produces<IReadOnlyList<GetProductCategoryQueryResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization();

        #endregion

        return app;
    }
}
