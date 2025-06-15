using MediatR;
namespace Stock_Maintenance_System_Application.Product.CreateCommand;
public record ProductCreateCommand(
    string ProductIdName,
    string ProductCompany,
    string? ProductModel,
    decimal? Mrp,
    decimal? SalePrice,
    int Quantity,
    int TotalQuantity,
    DateTime? PurchaseDate,
    bool IsActive,
    int? CreatedBy,
    DateTime? CreatedDate,
    int? ModifiedBy,
    DateTime? ModifiedDate
) : IRequest<int>
{
    public string ItemName => $"{ProductIdName} {ProductCompany}";
}
