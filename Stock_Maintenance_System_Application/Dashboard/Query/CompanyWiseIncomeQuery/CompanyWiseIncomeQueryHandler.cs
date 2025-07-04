using InventorySystem_Domain;
using InventorySystem_Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem_Application.Dashboard.Query.CompanyWiseIncomeQuery;
internal class CompanyWiseIncomeQueryHandler : IRequestHandler<CompanyWiseIncomeQuery, IReadOnlyList<CompanyWiseIncomeQueryResponse>>
{
    private readonly IRepository<InventorySystem_Domain.Category> _categoryRepository;
    private readonly IRepository<InventorySystem_Domain.Product> _productRepository;
    private readonly IRepository<InventorySystem_Domain.OrderItem> _orderItemRepository;
    private readonly IRepository<InventorySystem_Domain.ProductCategory> _productCategoryRepository;
    private readonly IRepository<InventorySystem_Domain.Company> _companyRepository;

    public CompanyWiseIncomeQueryHandler(IRepository<InventorySystem_Domain.Category> categoryRepository, IRepository<InventorySystem_Domain.Product> productRepository, IRepository<OrderItem> orderItemRepository, IRepository<InventorySystem_Domain.ProductCategory> productCategoryRepository, IRepository<InventorySystem_Domain.Company> companyRepository)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _orderItemRepository = orderItemRepository;
        _productCategoryRepository = productCategoryRepository;
        _companyRepository = companyRepository;
    }

    public async Task<IReadOnlyList<CompanyWiseIncomeQueryResponse>> Handle(CompanyWiseIncomeQuery request, CancellationToken cancellationToken)
    {
        var result = await _orderItemRepository.Table
                .Where(ord =>
                ord.CreatedAt.Month == DateTime.Now.AddMonths(-1).Month &&
                ord.CreatedAt.Year == DateTime.Now.Year)
        .Join(_productRepository.Table,
                ord => ord.ProductId,
                pro => pro.ProductId,
                (ord, pro) => new { ord, pro })
        .Join(_categoryRepository.Table,
                op => op.pro.CategoryId,
                cat => cat.CategoryId,
                (op, cat) => new { op.ord, op.pro, cat })
        .Join(_productCategoryRepository.Table,
                opc => opc.pro.ProductCategoryId,
                pcat => pcat.ProductCategoryId,
                (opc, pcat) => new { opc.ord, opc.pro, opc.cat, pcat })
        .Join(_companyRepository.Table,
                oppc => oppc.cat.CompanyId,
                com => com.CompanyId,
                (oppc, com) => new
                {
                    com.CompanyName,
                    oppc.cat.CategoryName,
                    oppc.pcat.ProductCategoryName,
                    oppc.ord.Quantity,
                    Income = (oppc.pro.MRP - oppc.ord.UnitPrice) * oppc.ord.Quantity
                })
        .GroupBy(x => new
        {
            x.CompanyName,
            x.CategoryName,
            x.ProductCategoryName
        })
        .Select(g => new CompanyWiseIncomeQueryResponse(
                g.Key.CompanyName,
                g.Key.CategoryName,
                g.Key.ProductCategoryName,
                g.Sum(x => x.Quantity),
                g.Sum(x => x.Income)
        )).OrderBy(x => x.CompanyName)
        .ThenBy(x => x.CategoryName)
        .ThenBy(x => x.ProductCategoryName)
        .ToListAsync();

        return result;
    }
}
