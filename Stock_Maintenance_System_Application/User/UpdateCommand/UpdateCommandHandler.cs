using MediatR;
using InventorySystem_Domain.Common;

namespace InventorySystem_Application.User.UpdateCommand
{
    internal sealed class UpdateCommandHandler : IRequestHandler<UpdateCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork; 
        public UpdateCommandHandler(IUnitOfWork unitOfWork ) => _unitOfWork = unitOfWork; 
        public async Task<bool> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            var userRepository = _unitOfWork.Repository<InventorySystem_Domain.User>();
            var user = await userRepository.GetByAsync(u => u.UserId == request.UserId);
            if (user == null)
                return false;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            bool isSuccess = false;
            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                isSuccess = await _unitOfWork.SaveAsync() > 0;
            }, cancellationToken);
            return isSuccess;
        }
    }
}
