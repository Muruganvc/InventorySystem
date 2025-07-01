namespace InventorySystem_Application.Category.Query.GetCategoriesQuery;
public record GetCategoryQueryResponse(int CompanyId, string CompanyName,
    int CategoryId, string CategoryName, string Description, bool IsActive, DateTime CreatedAt,
    string CreatedBy); 