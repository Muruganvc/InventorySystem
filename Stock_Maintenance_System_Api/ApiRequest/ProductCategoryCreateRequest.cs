namespace Stock_Maintenance_System_Api.ApiRequest;
public record ProductCategoryCreateRequest(int CategoryId, string CategoryProductName, string Description, bool IsActive);