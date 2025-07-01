namespace InventorySystem_Application.User.GetMenuItemPermissionQuery;
public class GetMenuItemPermissionQueryResponse
{
    public int Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public bool HasPermission { get; set; }
}