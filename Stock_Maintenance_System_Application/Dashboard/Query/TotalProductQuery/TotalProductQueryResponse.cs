namespace InventorySystem_Application.Dashboard.Query.TotalProductQuery;

public record CompanyWiseSales(
       int CompanyId,
       string CompanyName,
       decimal TotalQuantity,
       decimal TotalNetTotal
   ); 
public record TotalProductQueryResponse(
    decimal TotalQuantity,
    decimal TotalNetAmount,
    decimal BalanceAmount,
    List<CompanyWiseSales> CompanyWiseSales
);