using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.InventoryCompanyInfo.UpdateInventoryCompanyInfoCommand;
public record UpdateInventoryCompanyInfoCommand(
    int InventoryCompanyInfoId,
    string InventoryCompanyInfoName,
    string Description,
    string Address,
    string MobileNo,
    string GstNumber,
    string ApiVersion,
    string UiVersion,
    byte[] QcCode,
    string Email,
    string BankName,
    string BankBranchName,
    string BankAccountNo,
    string BankBranchIFSC) : IRequest<IResult<bool>>;
