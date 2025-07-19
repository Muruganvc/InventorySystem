using InventorySystem_Domain.Common;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace InventorySystem_Application.Common;
public sealed class UserInfo : IUserInfo
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UserInfo(IHttpContextAccessor httpContextAccessor) =>
        _httpContextAccessor = httpContextAccessor;
    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
    public string UserId => User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0";
    public string Email => User?.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
    public string UserName => User?.Identity?.Name ?? "System";
}
