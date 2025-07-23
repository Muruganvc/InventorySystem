using InventorySystem_Application.Common;
using InventorySystem_Domain.Common;
using MediatR;

namespace InventorySystem_Application.User.ActiveUserCommand;

internal sealed class ActiveUserCommandHandler : IRequestHandler<ActiveUserCommand, IResult<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<InventorySystem_Domain.User> _userRepository;
    private readonly IUserInfo _userInfo;
    public ActiveUserCommandHandler(IUnitOfWork unitOfWork,
        IRepository<InventorySystem_Domain.User> userRepository,  IUserInfo userInfo)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _userInfo = userInfo;
    }
    public async Task<IResult<bool>> Handle(ActiveUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByAsync(a => a.UserId == request.UserId);
        if (user == null) return Result<bool>.Failure("Selected user not found");
        user.IsActive = !user.IsActive;
        user.ModifiedBy = _userInfo.UserId;
        user.ModifiedDate = DateTime.Now;
        bool isSuccess = false;
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            isSuccess = await _unitOfWork.SaveAsync() > 0;
        }, cancellationToken);
        return Result<bool>.Success(isSuccess);
    }
}
