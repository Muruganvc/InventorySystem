using MediatR;

namespace InventorySystem_Application.Product.Query.GetProducts;
//type like sales or product
public record GetProductsQuery(string type)  
     : IRequest<IReadOnlyList<GetProductsQueryResponse>>;
