using InventorySystem_Application.Common;
using InventorySystem_Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem_Application.InventoryCompanyInfo.UpdateInventoryCompanyInfoCommand
{
    internal class UpdateInventoryCompanyInfoCommandHandler : IRequestHandler<UpdateInventoryCompanyInfoCommand, IResult<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<InventorySystem_Domain.InventoryCompanyInfo> _invCompanyInfoRepository;
        public UpdateInventoryCompanyInfoCommandHandler(IUnitOfWork unitOfWork, IRepository<InventorySystem_Domain.InventoryCompanyInfo> invCompanyInfoRepository)
        {
            _unitOfWork = unitOfWork;
            _invCompanyInfoRepository = invCompanyInfoRepository;
        }

        public async Task<IResult<bool>> Handle(UpdateInventoryCompanyInfoCommand request, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.Repository<InventorySystem_Domain.InventoryCompanyInfo>();
            var companyInfo = await repository.Table.FirstOrDefaultAsync(cancellationToken);

            if (companyInfo is null)
            {
                return Result<bool>.Failure("Selected company not found.");
            }

            // Map properties from request to entity
            companyInfo.InventoryCompanyInfoName = request.InventoryCompanyInfoName;
            companyInfo.Description = request.Description;
            companyInfo.Address = request.Address;
            companyInfo.MobileNo = request.MobileNo;
            companyInfo.GstNumber = request.GstNumber;
            companyInfo.BankName = request.BankName;
            companyInfo.BankBranchName = request.BankBranchName;
            companyInfo.BankAccountNo = request.BankAccountNo;
            companyInfo.BankBranchIFSC = request.BankBranchIFSC;
            companyInfo.ApiVersion = request.ApiVersion;
            companyInfo.UiVersion = request.UiVersion;

            // Update QcCode if provided
            if (request.QcCode is { Length: > 0 })
            {
                companyInfo.QcCode = request.QcCode;
            }

            var isSuccess = false;

            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                isSuccess = await _unitOfWork.SaveAsync() > 0;
            }, cancellationToken);

            return Result<bool>.Success(isSuccess);
        }
    }
}