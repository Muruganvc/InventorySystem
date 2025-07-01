
namespace Stock_Maintenance_System_Domain;
public class MenuItem
{
    public int Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
    public int? ParentId { get; set; }
    public MenuItem? Parent { get; set; }
    public int? OrderBy { get; set; }
    public ICollection<MenuItem>? Children { get; set; }
}