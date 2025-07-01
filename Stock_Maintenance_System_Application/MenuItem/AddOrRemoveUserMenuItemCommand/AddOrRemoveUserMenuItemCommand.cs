using MediatR;

namespace Stock_Maintenance_System_Application.MenuItem.AddOrRemoveUserMenuItemCommand;

public record AddOrRemoveUserMenuItemCommand(int UserId, int MenuId) : 
    IRequest<bool>;
 