namespace InventorySystem_Api.ApiRequest;
public record CategoryCreateRequest(int CompanyId,string CategoryName, string Description, bool IsActive);