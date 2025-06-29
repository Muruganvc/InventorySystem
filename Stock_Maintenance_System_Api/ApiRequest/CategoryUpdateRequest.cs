namespace Stock_Maintenance_System_Api.ApiRequest;
public record CategoryUpdateRequest(int CategoryId, int CompanyId, string CategoryName, string Description, bool IsActive);
