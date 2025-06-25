using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stock_Maintenance_System_Domain.Common;
using System.Security.Claims;

namespace Stock_Maintenance_System_Application.Product.Query.GetProducts;

internal sealed class GetProductsQueryHandler
    : IRequestHandler<GetProductsQuery, IReadOnlyList<GetProductsQueryResponse>>
{
    private readonly IRepository<Stock_Maintenance_System_Domain.Product> _productRepository;
   private readonly IHttpContextAccessor _httpContextAccessor;
    public GetProductsQueryHandler(IRepository<Stock_Maintenance_System_Domain.Product> productRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _productRepository = productRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    //private string GetProductName(Stock_Maintenance_System_Domain.Product product)
    //{
    //    var companyName = product.Company?.CompanyName ?? string.Empty;
    //    var categoryName = product.Category?.CategoryName ?? string.Empty;
    //    var productName = product.ProductName ?? string.Empty;
    //    return $"{product.Company?.CompanyName ?? string.Empty} {product.Category?.CategoryName ?? string.Empty} {product.ProductName ?? string.Empty}".Trim();
    //}
    public async Task<IReadOnlyList<GetProductsQueryResponse>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        var username = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
        var email = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
        var query = _productRepository.Table.AsQueryable();
        if (1 == 1)
        {
            query = query.Where(p => p.IsActive);
        }
        var result = await query
           .Where(a => a.IsActive)
    .Select(pro => new
    {
        pro.ProductId,
        pro.ProductName,
        pro.ProductCategoryId,
        ProductCategoryName = pro.ProductCategory != null ? pro.ProductCategory.ProductCategoryName : null,
        pro.CategoryId,
        CategoryName = pro.Category != null ? pro.Category.CategoryName : null,
        pro.CompanyId,
        CompanyName = pro.Company != null ? pro.Company.CompanyName : null,
        pro.Description,
        pro.MRP,
        pro.SalesPrice,
        pro.Quantity,
        pro.TaxPercent,
        pro.TaxType,
        pro.Barcode,
        pro.BrandName,
        pro.IsActive,
        CreatedByUser = pro.CreatedByUser != null ? pro.CreatedByUser.Username : null
    })
    .ToListAsync(cancellationToken);

        // Post-process with custom logic for ProductName
        var mappedResult = result.Select(pro => new GetProductsQueryResponse(
            pro.ProductId,
            $"{pro.CompanyName ?? ""} {pro.CategoryName ?? ""}" +
            (string.IsNullOrWhiteSpace(pro.ProductName) ? "" : $" {pro.ProductName}"),
            pro.ProductCategoryId,
            pro.ProductCategoryName,
            pro.CategoryId,
            pro.CategoryName,
            pro.CompanyId,
            pro.CompanyName,
            pro.Description,
            pro.MRP,
            pro.SalesPrice,
            pro.Quantity,
            pro.TaxPercent,
            pro.TaxType,
            pro.Barcode,
            pro.BrandName,
            pro.IsActive,
            pro.CreatedByUser
        )).ToList();

        return mappedResult;
    }
}