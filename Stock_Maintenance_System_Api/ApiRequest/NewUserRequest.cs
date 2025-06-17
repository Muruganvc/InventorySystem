public record NewUserRequest(
    string Id,
    string FirstName,
    string LastName,
    string UserName,
    string EmailId,
    string Password,
    string Role,
    bool IsActive,
    bool SuperAdmin
);

