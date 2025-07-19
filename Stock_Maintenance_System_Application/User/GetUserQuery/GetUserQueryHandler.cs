using MediatR;
using InventorySystem_Domain.Common;
using InventorySystem_Application.Common;

namespace InventorySystem_Application.User.GetUserQuery;

internal sealed class GetUserQueryHandler : IRequestHandler<GetUserQuery, IResult<GetUserQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserQueryHandler(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;

    public async Task<IResult<GetUserQueryResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork
            .Repository<InventorySystem_Domain.User>()
            .GetByAsync(u => u.Username == request.userName);

        if (user == null)
            return Result<GetUserQueryResponse>.Failure("User not found");

        string? base64Image = user.ProfileImage != null
            ? $"data:image/jpeg;base64,{Convert.ToBase64String(user.ProfileImage)}"
            : null;

        return Result<GetUserQueryResponse>.Success(new GetUserQueryResponse(
            FirstName: user.FirstName,
            LastName: user.LastName ?? string.Empty,
            UserName: user.Username,
            Email: user.Email ?? string.Empty,
            ProfileImageBase64: base64Image,
           MobileNo: user.MobileNo
        ));
    }
}

