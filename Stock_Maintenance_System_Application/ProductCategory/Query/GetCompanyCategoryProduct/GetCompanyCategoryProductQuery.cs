using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.ProductCategory.Query.GetCompanyCategoryProduct;
public record GetCompanyCategoryProductQuery():  
    IRequest<IResult<IReadOnlyList<KeyValuePair<string, string>>>>;
 