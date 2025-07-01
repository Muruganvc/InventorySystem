namespace InventorySystem_Application.ProductCategory.Query.GetProductCategoriesQuery;
public record GetProductCategoryQueryResponse(
    int CompanyId,
    string CompanyName,
    int CategoryId,
    string CategoryName,
    int ProductCategoryId,
    string ProductCategoryName,
    string Description,
    bool IsActive,
    DateTime CreatedAt,
    string Username
);
