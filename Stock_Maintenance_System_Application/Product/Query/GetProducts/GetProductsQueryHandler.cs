using MediatR;
using Microsoft.EntityFrameworkCore;
using Stock_Maintenance_System_Domain.Common;
using System.Linq;

namespace Stock_Maintenance_System_Application.Product.Query.GetProducts;

internal sealed class GetProductsQueryHandler
    : IRequestHandler<GetProductsQuery, IReadOnlyList<GetProductsQueryResponse>>
{
    private readonly IRepository<Stock_Maintenance_System_Domain.Product> _productRepository;

    public GetProductsQueryHandler(IRepository<Stock_Maintenance_System_Domain.Product> productRepository)
    {
        _productRepository = productRepository;
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
        var result = await _productRepository.Table
            .Select(pro => new GetProductsQueryResponse(
                pro.ProductId,
                pro.ComputedProductName,
                pro.ProductCategory != null ? pro.ProductCategory.ProductCategoryId : (int?)null,
                pro.ProductCategory != null ? pro.ProductCategory.ProductCategoryName : null,
                pro.Category != null ? pro.Category.CategoryId : (int?)null,
                pro.Category != null ? pro.Category.CategoryName : null,
                pro.Company != null ? pro.Company.CompanyId : (int?)null,
                pro.Company != null ? pro.Company.CompanyName : null,
                pro.Description,
                pro.MRP,
                pro.SalesPrice,
                pro.Quantity,
                pro.TaxPercent,
                pro.TaxType,
                pro.Barcode,
                pro.BrandName,
                pro.IsActive,
                pro.CreatedByUser != null ? pro.CreatedByUser.Username : null
            ))
            .ToListAsync(cancellationToken);
        return result;
    }
}
