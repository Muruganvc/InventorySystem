using MediatR;
using Stock_Maintenance_System_Domain.Common;

namespace Stock_Maintenance_System_Application.User.GetUserQuery;

internal sealed class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserQueryHandler(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;

    public async Task<GetUserQueryResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork
            .Repository<Stock_Maintenance_System_Domain.User>()
            .GetByAsync(u => u.Username == request.userName);

        if (user == null)
            return new GetUserQueryResponse(string.Empty, string.Empty, string.Empty, string.Empty);

        return new GetUserQueryResponse(
            FirstName: user.FirstName,
            LastName: user.LastName ?? string.Empty,
            UserName: user.Username,
            Email: user.Email ?? string.Empty
        );
    }
}

