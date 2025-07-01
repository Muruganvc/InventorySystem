namespace Stock_Maintenance_System_Domain;
public class UserMenuPermission
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public  User? User { get; set; }
    public int OrderBy { get; set; }
    public int MenuItemId { get; set; }
    public MenuItem? MenuItem { get; set; }
}
