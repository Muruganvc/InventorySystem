namespace Stock_Maintenance_System_Application.Company.Query;
public record GetCompanyQueryResponse(int CompanyId,string CompanyName, string Description, bool IsActive, DateTime CreateDate, string CreatedBy);
