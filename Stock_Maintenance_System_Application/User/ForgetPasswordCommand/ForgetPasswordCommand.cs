using InventorySystem_Application.Common;
using MediatR;
namespace InventorySystem_Application.User.ForgetPasswordCommand;
public record ForgetPasswordCommand(string UserName, string MobileNo, string Password)
    : IRequest<IResult<bool>>;