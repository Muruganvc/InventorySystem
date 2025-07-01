using MediatR;
using InventorySystem_Domain.Common;

namespace InventorySystem_Application.User.PasswordChangeCommand;
internal sealed class PasswordChangeCommandHandler : IRequestHandler<PasswordChangeCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<InventorySystem_Domain.User> _userRepository;
    public PasswordChangeCommandHandler(IUnitOfWork unitOfWork,
        IRepository<InventorySystem_Domain.User> userRepository)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }
    public async Task<bool> Handle(PasswordChangeCommand request, CancellationToken cancellationToken)
    {
        //var user1 = _unitOfWork.Repository<InventorySystem_Domain.User.User>().Table;
        //var userRole = _unitOfWork.Repository<InventorySystem_Domain.User.Role>().Table;
        var user = await _userRepository.GetByAsync(u => u.UserId == request.UserId);
        if (user == null)
            return false;

        if (user.PasswordHash != request.CurrentPassword)
            return false;

        user.PasswordHash = request.PasswordHash;
        user.PasswordLastChanged = request.PasswordLastChanged;
        user.IsPasswordExpired = false;
        bool isSuccess = false;
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            isSuccess = await _unitOfWork.SaveAsync() > 0;
        }, cancellationToken);
        return isSuccess;
    }
}