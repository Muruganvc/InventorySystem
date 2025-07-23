using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.User.AddOrRemoveRoleCommand;
public record AddOrRemoveRoleCommand(int UserId, int RoleId):IRequest<IResult<bool>>;
