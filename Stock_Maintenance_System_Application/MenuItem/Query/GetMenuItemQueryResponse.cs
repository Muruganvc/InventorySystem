namespace Stock_Maintenance_System_Application.MenuItem.Query;
internal class GetMenuItemQueryResponse
{
    public string Label { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string? Route { get; set; }
    public List<GetMenuItemQueryResponse>? SubMenuItem { get; set; }
}
