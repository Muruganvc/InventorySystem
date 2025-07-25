﻿namespace InventorySystem_Application.User.GetUserQuery;
public record GetUserQueryResponse(
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string? ProfileImageBase64,
    string MobileNo
);

