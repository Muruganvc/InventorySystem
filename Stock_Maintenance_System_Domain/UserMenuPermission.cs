namespace Stock_Maintenance_System_Domain;
public class UserMenuPermission
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public  User User { get; set; } = new User();
    public int MenuItemId { get; set; }
    public MenuItem? MenuItem { get; set; }
}
