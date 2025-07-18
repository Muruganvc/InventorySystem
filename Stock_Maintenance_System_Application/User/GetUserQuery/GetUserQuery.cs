using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.User.GetUserQuery;
public record GetUserQuery(string userName) : IRequest<IResult<GetUserQueryResponse>>;