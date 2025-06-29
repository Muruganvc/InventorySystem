namespace Stock_Maintenance_System_Api.ApiRequest;
public record ProductCategoryUpdateRequest( int ProductCategoryId, int CategoryId, string CategoryName, string Description, bool IsActive);
