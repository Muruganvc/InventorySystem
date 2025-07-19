namespace InventorySystem_Domain.Common;
public interface IUserInfo
{ 
    string UserId { get; }
    string Email { get; }
    string UserName { get; }
}
