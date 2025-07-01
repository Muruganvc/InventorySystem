namespace InventorySystem_Api.ApiRequest;
public record ProductCategoryUpdateRequest( int ProductCategoryId, int CategoryId, string ProductCategoryName, string Description, bool IsActive);
