using InventorySystem_Application.Common;
using InventorySystem_Domain;
using InventorySystem_Domain.Common;
using MediatR;

namespace InventorySystem_Application.User.AddOrRemoveRoleCommand
{
    internal sealed class AddOrRemoveRoleCommandHandler : IRequestHandler<AddOrRemoveRoleCommand, IResult<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddOrRemoveRoleCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
        public async Task<IResult<bool>> Handle(AddOrRemoveRoleCommand request, CancellationToken cancellationToken)
        {
            var userRoleRepository = _unitOfWork.Repository<UserRole>();

            var existingRole = await userRoleRepository
                .GetByAsync(ur => ur.UserId == request.UserId && ur.RoleId == request.RoleId);

            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                if (existingRole is null)
                {
                    var newUserRole = new UserRole
                    {
                        UserId = request.UserId,
                        RoleId = request.RoleId
                    };
                    await userRoleRepository.AddAsync(newUserRole);
                }
                else
                {
                    userRoleRepository.Delete(existingRole);
                }
                await _unitOfWork.SaveAsync();
            }, cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}