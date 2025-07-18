using InventorySystem_Application.Common;
using InventorySystem_Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace InventorySystem_Application.Company.Command.CreateCommand
{
    internal sealed class CompanyCreateCommandHandler : IRequestHandler<CompanyCreateCommand, IResult<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<InventorySystem_Domain.Company> _companyRepository;
        public CompanyCreateCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IRepository<InventorySystem_Domain.Company> companyRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _companyRepository = companyRepository;
        }
        public async Task<IResult<int>> Handle(CompanyCreateCommand request, CancellationToken cancellationToken)
        {
            int userId = int.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;

            var IsExistCompany = await _companyRepository.GetByAsync(a => a.CompanyName == request.CompanyName);
            if (IsExistCompany != null)
                return Result<int>.Failure("Entered company already exists");

            var company = new InventorySystem_Domain.Company
            {
                CompanyName = request.CompanyName,
                Description = request.Description,
                IsActive = request.IsActive,
                CreatedBy= userId,
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
