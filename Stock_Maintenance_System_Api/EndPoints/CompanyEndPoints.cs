using InventorySystem_Api.ApiRequest;
using InventorySystem_Application.Category.Command.CreateCommand;
using InventorySystem_Application.Category.Command.UpdateCommand;
using InventorySystem_Application.Category.Query.GetCategoriesQuery;
using InventorySystem_Application.Category.Query.GetCategoryQuery;
using InventorySystem_Application.Company.Command.BulkCompanyCommand;
using InventorySystem_Application.Company.Command.CreateCommand;
using InventorySystem_Application.Company.Command.UpdateCommand;
using InventorySystem_Application.Company.Query;
using InventorySystem_Application.ProductCategory.Command.CreateCommand;
using InventorySystem_Application.ProductCategory.Command.UpdateCommand;
using InventorySystem_Application.ProductCategory.Query.GetProductCategoriesQuery;
using InventorySystem_Application.ProductCategory.Query.GetProductCategoryQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem_Api.EndPoints;

public static class CompanyEndPoints
{
    public static IEndpointRouteBuilder MapProductCompanyEndpoints(this IEndpointRouteBuilder app)
    {
        #region Company

        app.MapPost("/company", async ([FromBody] CompanyCreateRequest company, IMediator mediator) =>
        {
            var command = new CompanyCreateCommand(company.CompanyName, company.Description, company.IsActive);
            var result = await mediator.Send(command);
            return Results.Ok(result);
        })
        .WithName("CreateCompany")
        .WithTags("Company")
        .Produces<int>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization("AllRoles");

        app.MapPut("/company/{companyId:int}", async (int companyId, [FromBody] CompanyUpdateRequest company, IMediator mediator) =>
        {
            var command = new CompanyUpdateCommand(companyId, company.CompanyName, company.Description, company.IsActive);
            var result = await mediator.Send(command);
            return Results.Ok(result);
        })
        .WithName("UpdateCompany")
        .WithTags("Company")
        .Produces<bool>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization("AllRoles");

        app.MapGet("/company", async ([FromQuery] bool isAllActiveCompany, [FromQuery] string? companyName, IMediator mediator) =>
        {
            var query = new GetCompanyQuery(isAllActiveCompany, companyName);
            var result = await mediator.Send(query);
            return Results.Ok(result);
        })
        .WithName("GetCompanyList")
        .WithTags("Company")
        .Produces<IReadOnlyList<KeyValuePair<string, int>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization("AllRoles");

        #endregion

        #region Category

        app.MapPost("/category", async ([FromBody] CategoryCreateRequest category, IMediator mediator) =>
        {
            var command = new CategoryCreateCommand(category.CompanyId, category.CategoryName, category.Description, category.IsActive);
            var result = await mediator.Send(command);
            return Results.Ok(result);
        })
        .WithName("CreateCategory")
        .WithTags("Category")
        .Produces<int>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization("AllRoles");

        app.MapPut("/category/{categoryId:int}", async (int categoryId, [FromBody] CategoryUpdateRequest category, IMediator mediator) =>
        {
            var command = new CategoryUpdateCommand(categoryId, category.CompanyId, category.CategoryName, category.Description, category.IsActive);
            var result = await mediator.Send(command);
            return Results.Ok(result);
        })
        .WithName("UpdateCategory")
        .WithTags("Category")
        .Produces<bool>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization("AllRoles");

        app.MapGet("/category/{companyId:int}", async (int companyId, IMediator mediator) =>
        {
            var query = new GetCategoryQuery(companyId);
            var result = await mediator.Send(query);
            return Results.Ok(result);
        })
        .WithName("GetCategoriesByCompany")
        .WithTags("Category")
        .Produces<IReadOnlyList<KeyValuePair<string, int>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
       .RequireAuthorization("AllRoles");

        app.MapGet("/categories", async ([FromQuery] bool isAllActive, IMediator mediator) =>
        {
            var query = new GetCategoriesQuery(isAllActive);
            var result = await mediator.Send(query);
            return Results.Ok(result);
        })
        .WithName("GetAllCategories")
        .WithTags("Category")
        .Produces<IReadOnlyList<GetCategoryQueryResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization("AllRoles");

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
            return Results.Ok(result);
        })
        .WithName("CreateProductCategory")
        .WithTags("ProductCategory")
        .Produces<int>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization("AllRoles");

        app.MapPut("/product-category/{productCategoryId:int}", async (int productCategoryId, [FromBody] ProductCategoryUpdateRequest productCategory, IMediator mediator) =>
        {
            var command = new ProductCategoryUpdateCommand(
                productCategoryId,
                productCategory.CategoryId,
                productCategory.ProductCategoryName,
                productCategory.Description,
                productCategory.IsActive);
            var result = await mediator.Send(command);
            return Results.Ok(result);
        })
        .WithName("UpdateProductCategory")
        .WithTags("ProductCategory")
        .Produces<bool>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
       .RequireAuthorization("AllRoles");

        app.MapGet("/product-category/{categoryId:int}", async (int categoryId, IMediator mediator) =>
        {
            var query = new GetProductCategoryQuery(categoryId);
            var result = await mediator.Send(query);
            return Results.Ok(result);
        })
        .WithName("GetProductCategoriesByCategory")
        .WithTags("ProductCategory")
        .Produces<IReadOnlyList<KeyValuePair<string, int>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
       .RequireAuthorization("AllRoles");

        app.MapGet("/product-categories", async ([FromQuery] bool isAllActive, IMediator mediator) =>
        {
            var query = new GetProductCategoriesQuery(isAllActive);
            var result = await mediator.Send(query);
            return Results.Ok(result);
        })
        .WithName("GetProductCategoriesQuery")
        .WithTags("ProductCategory")
        .Produces<IReadOnlyList<GetProductCategoryQueryResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization("AllRoles");

        #endregion

        app.MapPost("/bulk-company", async (
        List<BulkComapanyRequest> request,
        IMediator mediator) =>
                { 
                    var bulkCompanyEntries = new List<BulkCompanyEntry>();
                    request.ForEach(a =>
                    {
                        bulkCompanyEntries.Add(new BulkCompanyEntry(
                            a.CompanyName,
                            a.CategoryName,
                            a.ProductCategoryName
                        ));
                    });
                    var command = new BulkCompanyCommand(bulkCompanyEntries);
                    var result = await mediator.Send(command);

                    return Results.Ok(result);
                })
        .WithName("BulkCompanyCommand")
        .WithTags("Products")
        .Produces(StatusCodes.Status200OK, typeof(object))
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization("AllRoles");

        return app;
    }
}
