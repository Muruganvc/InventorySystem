using InventorySystem_Application.Common;
using InventorySystem_Domain.Common;
using MediatR;

namespace InventorySystem_Application.InventoryCompanyInfo.CreateInventoryCompanyInfoCommand;
internal sealed class CreateInventoryCompanyInfoCommandHandler : IRequestHandler<CreateInventoryCompanyInfoCommand, IResult<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    public CreateInventoryCompanyInfoCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
    public async Task<IResult<bool>> Handle(CreateInventoryCompanyInfoCommand request, CancellationToken cancellationToken)
    {
        if (request.QcCode.Length == 0) return Result<bool>.Failure("Please upload valid QrCode");
        var inventoryCompanyInfo = new InventorySystem_Domain.InventoryCompanyInfo
        {
            Address = request.Address,
            Description = request.Description,
            ApiVersion = request.ApiVersion,
            Email = request.Email,
            GstNumber = request.GstNumber,
            InventoryCompanyInfoName = request.InventoryCompanyInfoName,
            MobileNo = request.MobileNo,
            QcCode = request.QcCode,
            UiVersion = request.UiVersion,
            BankName = request.BankName,
            BankBranchName = request.BankBranchName,
            BankAccountNo = request.BankAccountNo,
            BankBranchIFSC = request.BankBranchIFSC,
        };
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            await _unitOfWork.Repository<InventorySystem_Domain.InventoryCompanyInfo>().AddAsync(inventoryCompanyInfo);
            await _unitOfWork.SaveAsync();
        }, cancellationToken);

        var isSuccess = inventoryCompanyInfo.InventoryCompanyInfoId > 0;
        return Result<bool>.Success(isSuccess);
    }
}