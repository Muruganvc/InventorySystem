using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using InventorySystem_Domain.Common;
using System.Security.Claims;
using InventorySystem_Application.Common;

namespace InventorySystem_Application.Product.Query.GetProducts;

internal sealed class GetProductsQueryHandler
    : IRequestHandler<GetProductsQuery, IResult<IReadOnlyList<GetProductsQueryResponse>>>
{
    private readonly IRepository<InventorySystem_Domain.Product> _productRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public GetProductsQueryHandler(IRepository<InventorySystem_Domain.Product> productRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _productRepository = productRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IResult<IReadOnlyList<GetProductsQueryResponse>>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        var username = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
        var email = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
        var roles = _httpContextAccessor.HttpContext?.User?.Claims.Where(c => c.Type == ClaimTypes.Role)
        .Select(c => c.Value).ToList();
        var query = _productRepository.Table.AsQueryable();
        bool isAdmin = roles?.Contains("Admin") ?? false;
        bool isManager = roles?.Contains("Manager") ?? false;
        bool isSalesScreen = request.type == "sales";
        if ((!isAdmin && !isManager) || isSalesScreen)
        {
            query = query.Where(p => p.IsActive);
        }
        var result = await query.Select(pro => new
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
            pro.LandingPrice,
            pro.Quantity,
            pro.IsActive,
            CreatedByUser = pro.CreatedByUser != null ? pro.CreatedByUser.Username : null,
            pro.SerialNo
        })
            .ToListAsync(cancellationToken);

        // Post-process with custom logic for ProductName
        var mappedResult = result.Select(pro => new GetProductsQueryResponse(
            pro.ProductId,
            pro.ProductName,
            pro.ProductCategoryId,
            pro.ProductCategoryName,
            pro.CategoryId,
            pro.CategoryName,
            pro.CompanyId,
            pro.CompanyName,
            pro.Description,
            pro.MRP,
            pro.SalesPrice,
            pro.LandingPrice,
            pro.Quantity,
            pro.IsActive,
            pro.CreatedByUser,
            pro.SerialNo, $"{pro.CompanyName} {pro.CategoryName} {pro.ProductCategoryName}",
            $"{pro.CompanyId}${pro.CategoryId}${pro.ProductCategoryId}"
        )).OrderBy(a => a.ProductName).ToList();
        return Result<IReadOnlyList<GetProductsQueryResponse>>.Success(mappedResult);
    }
}