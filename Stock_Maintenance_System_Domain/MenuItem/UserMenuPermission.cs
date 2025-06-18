namespace Stock_Maintenance_System_Domain.MenuItem;
public class UserMenuPermission
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public Stock_Maintenance_System_Domain.User.User User { get; set; } = new User.User();
    public int MenuItemId { get; set; }
    public MenuItem? MenuItem { get; set; }
}
