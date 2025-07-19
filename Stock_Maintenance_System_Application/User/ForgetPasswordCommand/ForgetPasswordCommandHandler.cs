using InventorySystem_Application.Common;
using InventorySystem_Domain.Common;
using MediatR;

namespace InventorySystem_Application.User.ForgetPasswordCommand;

internal sealed class ForgetPasswordCommandHandler
    : IRequestHandler<ForgetPasswordCommand, IResult<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<InventorySystem_Domain.User> _userRepository;
    public ForgetPasswordCommandHandler(IUnitOfWork unitOfWork, IRepository<InventorySystem_Domain.User> userRepository)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    public async Task<IResult<bool>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByAsync(u => u.Username == request.UserName);
        if (user == null)
            return Result<bool>.Failure("Invalid user name.");

        if (user.MobileNo != request.MobileNo)
            return Result<bool>.Failure("Invalid Mobile No.");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        user.PasswordLastChanged = DateTime.Now;
        user.IsPasswordExpired = false;

        var isSuccess = false;

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            isSuccess = await _unitOfWork.SaveAsync() > 0;
        }, cancellationToken);

        return Result<bool>.Success(isSuccess);
    }
}
