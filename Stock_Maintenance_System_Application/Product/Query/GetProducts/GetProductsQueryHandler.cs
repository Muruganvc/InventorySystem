using MediatR;
using Stock_Maintenance_System_Domain.Common;

namespace Stock_Maintenance_System_Application.Product.Query.GetProducts;
internal sealed class GetProductsQueryHandler
    : IRequestHandler<GetProductsQuery, IReadOnlyList<GetProductsQueryResponse>>
{
    private readonly IRepository<Stock_Maintenance_System_Domain.Company> _companyRepository;
    private readonly IRepository<Stock_Maintenance_System_Domain.Category> _categoryRepository;
    private readonly IRepository<Stock_Maintenance_System_Domain.Product> _productRepository;
    private readonly IRepository<Stock_Maintenance_System_Domain.User> _userRepository;

    public GetProductsQueryHandler(IRepository<Stock_Maintenance_System_Domain.Company> companyRepository,
        IRepository<Stock_Maintenance_System_Domain.Category> categoryRepository,
        IRepository<Stock_Maintenance_System_Domain.Product> productRepository,
        IRepository<Stock_Maintenance_System_Domain.User> userRepository)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _userRepository = userRepository;
        _companyRepository = companyRepository;
    }
    public async Task<IReadOnlyList<GetProductsQueryResponse>> Handle(
            GetProductsQuery request,
            CancellationToken cancellationToken)
    {
        return null;
    }
}