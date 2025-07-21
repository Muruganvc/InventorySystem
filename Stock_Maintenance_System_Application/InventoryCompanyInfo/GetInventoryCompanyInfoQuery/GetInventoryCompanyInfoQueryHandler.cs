using InventorySystem_Application.Common;
using InventorySystem_Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem_Application.InventoryCompanyInfo.GetInventoryCompanyInfoQuery;
internal sealed class GetInventoryCompanyInfoQueryHandler
    : IRequestHandler<GetInventoryCompanyInfoQuery, IResult<GetInventoryCompanyInfoQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetInventoryCompanyInfoQueryHandler(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;
    public async Task<IResult<GetInventoryCompanyInfoQueryResponse>> Handle(GetInventoryCompanyInfoQuery request, CancellationToken cancellationToken)
    {
        var companyInfo = await _unitOfWork
            .Repository<InventorySystem_Domain.InventoryCompanyInfo>()
            .Table
            .FirstOrDefaultAsync(cancellationToken);

        if (companyInfo == null)
            return Result<GetInventoryCompanyInfoQueryResponse>.Failure("Inventory Company information not found");

        var base64Image = companyInfo.QcCode != null
            ? $"data:image/jpeg;base64,{Convert.ToBase64String(companyInfo.QcCode)}"
            : null;

        var response = new GetInventoryCompanyInfoQueryResponse(
            InventoryCompanyInfoId: companyInfo.InventoryCompanyInfoId,
            InventoryCompanyInfoName: companyInfo.InventoryCompanyInfoName,
            Description: companyInfo.Description,
            Address: companyInfo.Address,
            MobileNo: companyInfo.MobileNo,
            GstNumber: companyInfo.GstNumber,
            ApiVersion: companyInfo.ApiVersion,
            UiVersion: companyInfo.UiVersion,
            QrCodeBase64: base64Image ?? string.Empty,
            Email: companyInfo.Email,
            BankName: companyInfo.BankName,
            BankBranchName: companyInfo.BankBranchName,
            BankAccountNo: companyInfo.BankAccountNo,
            BankBranchIFSC: companyInfo.BankBranchIFSC
        );
        return Result<GetInventoryCompanyInfoQueryResponse>.Success(response);
    }
}
