namespace InventorySystem_Application.User.GetUsersQuery;
public class GetUsersQueryResponse
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } =string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; }
    public bool IsActive { get; set; }
    public bool SuperAdmin { get; set; }
    public DateTime LastLogin { get; set; }

}
