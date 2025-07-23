using InventorySystem_Application.Common;
using InventorySystem_Domain;
using InventorySystem_Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace InventorySystem_Application.Dashboard.Query.AuditQuery;
internal class AuditQueryHandler : IRequestHandler<AuditQuery, IResult<IReadOnlyList<AuditQueryResponse>>>
{
    private readonly IRepository<AuditLog> _auditRepository;
    public AuditQueryHandler(IRepository<AuditLog> auditRepository) => _auditRepository = auditRepository; 
    public async Task<IResult<IReadOnlyList<AuditQueryResponse>>> Handle(AuditQuery request, CancellationToken cancellationToken)
    {
        var auditLogs = await _auditRepository.Table
            .ToListAsync(cancellationToken);

        var response = auditLogs.Select(a => new AuditQueryResponse
        {
            Id = a.Id,
            TableName = a.TableName!,
            Action = a.Action!,
            ChangedBy = a.ChangedBy!,
            ChangedAt = a.ChangedAt,
            KeyValues = DeserializeJson(a.KeyValues),
            OldValues = DeserializeJson(a.OldValues),
            NewValues = DeserializeJson(a.NewValues)
        }).ToList();

        return Result<IReadOnlyList<AuditQueryResponse>>.Success(response);
    }

    private static Dictionary<string, object>? DeserializeJson(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return null;

        return JsonSerializer.Deserialize<Dictionary<string, object>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
}
