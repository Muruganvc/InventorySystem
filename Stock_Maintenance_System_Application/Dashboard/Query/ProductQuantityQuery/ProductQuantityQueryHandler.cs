using InventorySystem_Domain.Common;
using InventorySystem_Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem_Application.Dashboard.Query.ProductQuantityQuery
{
    internal sealed class ProductQuantityQueryHandler
        : IRequestHandler<ProductQuantityQuery, IReadOnlyList<ProductQuantityQueryResponse>>
    {
        private readonly IRepository<InventorySystem_Domain.Category> _categoryRepository;
        private readonly IRepository<InventorySystem_Domain.Product> _productRepository;
        private readonly IRepository<InventorySystem_Domain.ProductCategory> _productCategoryRepository;
        private readonly IRepository<InventorySystem_Domain.Company> _companyRepository;

        public ProductQuantityQueryHandler(
            IRepository<InventorySystem_Domain.Category> categoryRepository,
            IRepository<InventorySystem_Domain.Product> productRepository,
            IRepository<InventorySystem_Domain.ProductCategory> productCategoryRepository,
            IRepository<InventorySystem_Domain.Company> companyRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
            _companyRepository = companyRepository;
        }

        public async Task<IReadOnlyList<ProductQuantityQueryResponse>> Handle(
          ProductQuantityQuery request,
          CancellationToken cancellationToken)
        {
            var result = await _companyRepository.Table
                .Join(_categoryRepository.Table,
                      company => company.CompanyId,
                      category => category.CompanyId,
                      (company, category) => new { company, category })
                .Join(_productCategoryRepository.Table,
                      x => x.category.CategoryId,
                      productCategory => productCategory.CategoryId,
                      (x, productCategory) => new { x.company, x.category, productCategory })
                .Join(_productRepository.Table,
                      x => x.productCategory.ProductCategoryId,
                      product => product.ProductCategoryId,
                      (x, product) => new ProductQuantityQueryResponse(
                          x.company.CompanyName,
                          x.category.CategoryName,
                          x.productCategory.ProductCategoryName,
                          product.Quantity))
                .ToListAsync(cancellationToken);

            return result;
        }

    }
}
