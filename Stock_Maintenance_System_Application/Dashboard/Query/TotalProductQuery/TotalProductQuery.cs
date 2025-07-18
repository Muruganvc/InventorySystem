using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.Dashboard.Query.TotalProductQuery;

public record TotalProductQuery():IRequest<IResult<TotalProductQueryResponse>>;
