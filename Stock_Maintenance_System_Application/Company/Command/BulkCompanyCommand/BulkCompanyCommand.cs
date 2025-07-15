using MediatR;

namespace InventorySystem_Application.Company.Command.BulkCompanyCommand;
public record BulkCompanyEntry(
   string CompanyName,
   string CategoryName,
   string ProductCategory
);
public record BulkCompanyCommand(List<BulkCompanyEntry> BulkCompanyCommands) : IRequest<bool>;
