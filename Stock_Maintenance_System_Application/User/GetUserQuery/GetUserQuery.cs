using MediatR;

namespace Stock_Maintenance_System_Application.User.GetUserQuery;
public record GetUserQuery(string userName) : IRequest<GetUserQueryResponse>;