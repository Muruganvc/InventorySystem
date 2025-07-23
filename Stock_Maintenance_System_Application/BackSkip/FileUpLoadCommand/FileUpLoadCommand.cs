using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.BackSkip.FileUpLoadCommand;

public record FileUpLoadCommand(string UploadPath) : IRequest<IResult<bool>>;
 