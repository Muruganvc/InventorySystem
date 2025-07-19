using MediatR;
using InventorySystem_Domain.Common;
using InventorySystem_Application.Common;

namespace InventorySystem_Application.User.UpdateCommand
{
    internal sealed class UpdateCommandHandler : IRequestHandler<UpdateCommand, IResult<bool>>
    {
        private readonly IUnitOfWork _unitOfWork; 
        public UpdateCommandHandler(IUnitOfWork unitOfWork ) => _unitOfWork = unitOfWork;
        public async Task<IResult<bool>> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            var userRepository = _unitOfWork.Repository<InventorySystem_Domain.User>();
            var user = await userRepository.GetByAsync(u => u.UserId == request.UserId);

            if (user == null)
                return Result<bool>.Failure("Invalid user");
             
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.MobileNo= request.MobileNo;
             
            if (request.ImageData != null && request.ImageData.Length > 0)
            {
                user.ProfileImage = request.ImageData;
            }

            bool isSuccess = false;
            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                isSuccess = await _unitOfWork.SaveAsync() > 0;
            }, cancellationToken);

            return Result<bool>.Success(isSuccess);
        }

    }
}
