﻿using InventorySystem_Application.Common;
using MediatR;
namespace InventorySystem_Application.User.PasswordChangeCommand;
public record PasswordChangeCommand(
    int UserId,
    string Username,
    string CurrentPassword,
    string PasswordHash,
    DateTime PasswordLastChanged,
    int? ModifiedBy,
    DateTime? ModifiedDate
) : IRequest<IResult<bool>>
{
    public DateTime PasswordExpiresAt => PasswordLastChanged.AddDays(30);
}
