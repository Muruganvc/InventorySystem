using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.BackSkip.RestoreFileDataCommand;
public record RestoreFileDataCommand() : IRequest<IResult<bool>>;
