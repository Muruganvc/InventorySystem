namespace InventorySystem_Application.Dashboard.Query.AuditQuery;
public class AuditQueryResponse
{
    public int Id { get; set; }
    public string TableName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string ChangedBy { get; set; } = string.Empty;
    public DateTime ChangedAt { get; set; }

    public Dictionary<string, object>? KeyValues { get; set; }
    public Dictionary<string, object>? OldValues { get; set; }
    public Dictionary<string, object>? NewValues { get; set; }
}
