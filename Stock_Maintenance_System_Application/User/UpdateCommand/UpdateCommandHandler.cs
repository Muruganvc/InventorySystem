using MediatR;
using Stock_Maintenance_System_Domain.Common;

namespace Stock_Maintenance_System_Application.User.UpdateCommand
{
    internal sealed class UpdateCommandHandler : IRequestHandler<UpdateCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork; 
        public async Task<bool> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            var userRepository = _unitOfWork.Repository<Stock_Maintenance_System_Domain.User>();
            var user = await userRepository.GetByAsync(u => u.UserId == request.UserId);

            if (user == null)
                return false;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.IsActive = request.IsActive;
            bool isSuccess = false;
            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                isSuccess = await _unitOfWork.SaveAsync() > 0;
            });
            return isSuccess;
        }
    }
}
