using MediatR;

namespace Stock_Maintenance_System_Application.Product.Query.GetProducts;
public record GetProductsQuery() 
     : IRequest<IReadOnlyList<GetProductsQueryResponse>>;
