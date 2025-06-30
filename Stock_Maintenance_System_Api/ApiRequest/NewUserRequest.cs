public record NewUserRequest(
    string FirstName,
    string LastName,
    string UserName,
    string EmailId,
    string Password,
    int Role,
    bool IsActive
);
