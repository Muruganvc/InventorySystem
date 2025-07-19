using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.Dashboard.Query.AuditQuery;
public record AuditQuery():IRequest<IResult<IReadOnlyList<AuditQueryResponse>>>;
