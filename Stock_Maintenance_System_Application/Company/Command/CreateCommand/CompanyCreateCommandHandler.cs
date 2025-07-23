using InventorySystem_Application.Common;
using InventorySystem_Domain.Common;
using MediatR;

namespace InventorySystem_Application.Company.Command.CreateCommand
{
    internal sealed class CompanyCreateCommandHandler : IRequestHandler<CompanyCreateCommand, IResult<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserInfo _userInfo;
        private readonly IRepository<InventorySystem_Domain.Company> _companyRepository;
        public CompanyCreateCommandHandler(IUnitOfWork unitOfWork, 
            IRepository<InventorySystem_Domain.Company> companyRepository, IUserInfo userInfo)
        {
            _unitOfWork = unitOfWork;
            _companyRepository = companyRepository;
            _userInfo = userInfo;
        }
        public async Task<IResult<int>> Handle(CompanyCreateCommand request, CancellationToken cancellationToken)
        {
            var IsExistCompany = await _companyRepository.GetByAsync(a => a.CompanyName == request.CompanyName);
            if (IsExistCompany != null)
                return Result<int>.Failure("Entered company already exists");

            var company = new InventorySystem_Domain.Company
            {
                CompanyName = request.CompanyName,
                Description = request.Description,
                IsActive = request.IsActive,
                CreatedBy= _userInfo.UserId,
                CreatedAt = DateTime.Now
            };
            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<InventorySystem_Domain.Company>().AddAsync(company);
                await _unitOfWork.SaveAsync();
            }, cancellationToken);
            return Result<int>.Success(company.CompanyId);
        }
    }
}
