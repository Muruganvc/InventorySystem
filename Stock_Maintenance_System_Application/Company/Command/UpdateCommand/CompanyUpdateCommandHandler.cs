using MediatR;
using InventorySystem_Domain.Common;
using InventorySystem_Application.Common;

namespace InventorySystem_Application.Company.Command.UpdateCommand;

public sealed class CompanyUpdateCommandHandler : IRequestHandler<CompanyUpdateCommand, IResult<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<InventorySystem_Domain.Company> _companyRepository;
    public CompanyUpdateCommandHandler(IUnitOfWork unitOfWork, IRepository<InventorySystem_Domain.Company> companyRepository)
    {
        _unitOfWork = unitOfWork;
        _companyRepository = companyRepository;
    }
    public async Task<IResult<bool>> Handle(CompanyUpdateCommand request, CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetByAsync(a => a.CompanyId == request.CompanyId);
        if (company == null)
            return Result<bool>.Failure("Selected company not found.");

        company.CompanyName = request.CompanyName;
        company.Description = request.Description;
        company.IsActive = request.IsActive;
        bool isSuccess = false;
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            isSuccess = await _unitOfWork.SaveAsync() > 0;
        }, cancellationToken);
        return Result<bool>.Success(isSuccess);
    }
}