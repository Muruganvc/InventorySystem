using MediatR;
using InventorySystem_Domain;
using InventorySystem_Domain.Common;
using InventorySystem_Application.Common;

namespace InventorySystem_Application.MenuItem.AddOrRemoveUserMenuItemCommand;
internal sealed class AddOrRemoveUserMenuItemCommandHandler
    : IRequestHandler<AddOrRemoveUserMenuItemCommand, IResult<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddOrRemoveUserMenuItemCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
    public async Task<IResult<bool>> Handle(AddOrRemoveUserMenuItemCommand request, CancellationToken cancellationToken)
    {
        var userMenuRepo = _unitOfWork.Repository<UserMenuPermission>();
        var existingPermission = await userMenuRepo.GetByAsync(
            u => u.UserId == request.UserId && u.MenuItemId == request.MenuId);

        var userMenuItemRepo = _unitOfWork.Repository<InventorySystem_Domain.MenuItem>();
        var menuItem = await userMenuItemRepo.GetByAsync(a => a.Id == request.MenuId);
        int orderBy = 0;
        if (menuItem is not null)
            orderBy = menuItem.OrderBy ?? 0;

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                if (existingPermission is null)
                {
                    var newPermission = new UserMenuPermission
                    {
                        UserId = request.UserId,
                        MenuItemId = request.MenuId,
                        OrderBy = orderBy
                    };
                    await userMenuRepo.AddAsync(newPermission);
                }
                else
                {
                    userMenuRepo.Delete(existingPermission);
                }
                await _unitOfWork.SaveAsync();
            }, cancellationToken);
        return Result<bool>.Success(true); 
    }
}
