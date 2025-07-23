using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using InventorySystem_Domain.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InventorySystem_Application.Common;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem_Application.User.LoginCommand
{
    internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, IResult<LoginCommandResponse>>
    {
        private readonly IRepository<InventorySystem_Domain.User> _userRepository;
        private readonly IRepository<InventorySystem_Domain.UserRole> _userRoleRepository;
        private readonly IRepository<InventorySystem_Domain.Role> _roleRepository;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        public LoginCommandHandler(
            IRepository<InventorySystem_Domain.User> userRepository,
            IRepository<InventorySystem_Domain.UserRole> userRoleRepository,
            IConfiguration configuration, IUnitOfWork unitOfWork, IRepository<InventorySystem_Domain.Role> roleRepository
            )
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _userRoleRepository = userRoleRepository;
            _unitOfWork = unitOfWork;
            _roleRepository = roleRepository;
        }

        public async Task<IResult<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByAsync(u => u.Username == request.UserName && u.IsActive);
            if (user is null)
                return Result<LoginCommandResponse>.Failure("Invalid user name");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Result<LoginCommandResponse>.Failure("Invalid password");

            var userRoles = await _userRoleRepository.GetListByAsync(ur => ur.UserId == user.UserId);
            var roleIds = userRoles.Select(ur => ur.RoleId).ToList();

            var roles = await _roleRepository.GetListByAsync(r => roleIds.Contains(r.RoleId));
            var roleNames = roles.Select(r => r.Name).ToList();

            var token = GenerateJwtToken(
                user.Username,
                user.Email ?? string.Empty,
                roleNames,
                user.UserId
            );

            var companyInfo = await _unitOfWork
                .Repository<InventorySystem_Domain.InventoryCompanyInfo>()
                .Table
                .FirstOrDefaultAsync(cancellationToken);

            GetInventoryCompanyInfoQueryResponse? companyResponse = null;

            if (companyInfo is not null)
            {
                var base64Image = companyInfo.QcCode != null
                    ? $"data:image/jpeg;base64,{Convert.ToBase64String(companyInfo.QcCode)}"
                    : string.Empty;

                companyResponse = new GetInventoryCompanyInfoQueryResponse(
                    InventoryCompanyInfoId: companyInfo.InventoryCompanyInfoId,
                    InventoryCompanyInfoName: companyInfo.InventoryCompanyInfoName,
                    Description: companyInfo.Description,
                    Address: companyInfo.Address,
                    MobileNo: companyInfo.MobileNo,
                    GstNumber: companyInfo.GstNumber,
                    ApiVersion: companyInfo.ApiVersion,
                    UiVersion: companyInfo.UiVersion,
                    QrCodeBase64: base64Image,
                    Email: companyInfo.Email ?? string.Empty,
                    BankName: companyInfo.BankName,
                    BankBranchName: companyInfo.BankBranchName,
                    BankAccountNo: companyInfo.BankAccountNo,
                    BankBranchIFSC: companyInfo.BankBranchIFSC
                );
            }

            var response = new LoginCommandResponse(
                UserId: user.UserId,
                FirstName: user.FirstName,
                LastName: user.LastName ?? string.Empty,
                Email: user.Email ?? string.Empty,
                user.Username,
                Token: token,
                InvCompanyInfo: companyResponse
            );
            return Result<LoginCommandResponse>.Success(response);
        }
        private string GenerateJwtToken(string username, string email, List<string> roleNames, int userId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };
            foreach (var role in roleNames)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpiresInMinutes"]!));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}