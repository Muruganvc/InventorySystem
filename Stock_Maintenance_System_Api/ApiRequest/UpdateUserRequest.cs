namespace Stock_Maintenance_System_Api.ApiRequest;
internal record UpdateUserRequest(
    string FirstName, string LastName, string Email, string MobileNumber,
    bool IsActive, bool IsSuperAdmin);


internal record ChangePasswordRequest(string UserName, string PasswordHash, string CurrentPassword);