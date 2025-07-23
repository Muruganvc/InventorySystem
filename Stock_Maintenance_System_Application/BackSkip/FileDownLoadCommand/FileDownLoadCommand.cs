using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.BackSkip.FileDownLoadCommand;
public record FileDownLoadCommand(string DownloadPath) : IRequest<IResult<bool>>;
