namespace InventorySystem_Api.ApiRequest;
public record BulkComapanyRequest(string CompanyName, string CategoryName, string ProductCategoryName);
public record BulkCompanyRequest(List<BulkComapanyRequest> BulkComapanyRequests);