namespace InventorySystem_Api.ApiRequest;
public record ProductCategoryCreateRequest(int CategoryId, string CategoryProductName, string Description, bool IsActive);