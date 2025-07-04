using MediatR;

namespace InventorySystem_Application.Dashboard.Query.TotalProductQuery;

public record TotalProductQuery():IRequest<TotalProductQueryResponse>;
