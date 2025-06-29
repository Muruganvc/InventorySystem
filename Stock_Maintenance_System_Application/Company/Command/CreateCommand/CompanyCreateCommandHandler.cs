using MediatR;
using Microsoft.AspNetCore.Http;
using Stock_Maintenance_System_Domain;
using Stock_Maintenance_System_Domain.Common;
using System.Security.Claims;

namespace Stock_Maintenance_System_Application.Company.Command.CreateCommand
{
    internal sealed class CompanyCreateCommandHandler : IRequestHandler<CompanyCreateCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CompanyCreateCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(CompanyCreateCommand request, CancellationToken cancellationToken)
        {
            int userId = int.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
            var company = new Stock_Maintenance_System_Domain.Company
            {
                CompanyName = request.CompanyName,
                Description = request.Description,
                IsActive = request.IsActive,
                CreatedBy= userId,
                CreatedAt = DateTime.Now
            };
            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<Stock_Maintenance_System_Domain.Company>().AddAsync(company);
                await _unitOfWork.SaveAsync();
            }, cancellationToken);
            return company.CompanyId;
        }
    }
}
