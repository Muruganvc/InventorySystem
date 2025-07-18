using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.Dashboard.Query.ProductQuantityQuery;

public record ProductQuantityQuery() :IRequest<IResult<IReadOnlyList<ProductQuantityQueryResponse>>>;
 