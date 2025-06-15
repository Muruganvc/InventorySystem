using MediatR;
using Stock_Maintenance_System_Domain.Common;

namespace Stock_Maintenance_System_Application.User.PasswordChangeCommand;
internal sealed class PasswordChangeCommandHandler : IRequestHandler<PasswordChangeCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    public PasswordChangeCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<bool> Handle(PasswordChangeCommand request, CancellationToken cancellationToken)
    {
        var user1 = _unitOfWork.Repository<Stock_Maintenance_System_Domain.User.User>().Table;
        var userRole = _unitOfWork.Repository<Stock_Maintenance_System_Domain.User.Role>().Table;
        var user = await _unitOfWork.Repository<Stock_Maintenance_System_Domain.User.User>().GetByAsync(a => a.Username == request.Username);
        if (user == null)
            return false;
        user.PasswordHash = request.PasswordHash;
        user.PasswordLastChanged = request.PasswordLastChanged;
        user.IsPasswordExpired = false;
        _unitOfWork.Repository<Stock_Maintenance_System_Domain.User.User>().Update(user);
        await _unitOfWork.SaveAsync();
        return true;
    }
}