using MediatR;

namespace InventorySystem_Application.Dashboard.Query.ProductQuantityQuery;

public record ProductQuantityQuery() :IRequest<IReadOnlyList<ProductQuantityQueryResponse>>;
 