using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using InventorySystem_Domain.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InventorySystem_Application.User.LoginCommand
{
    internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginCommandResponse>
    {
        private readonly IRepository<InventorySystem_Domain.User> _userRepository;
        private readonly IRepository<InventorySystem_Domain.UserRole> _userRoleRepository;
        private readonly IConfiguration _configuration;

        public LoginCommandHandler(
            IRepository<InventorySystem_Domain.User> userRepository,
            IRepository<InventorySystem_Domain.UserRole> userRoleRepository,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _userRoleRepository= userRoleRepository;
        }

        public async Task<LoginCommandResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByAsync(u => u.Username == request.UserName && u.IsActive);

            if (user is null)
                throw new UnauthorizedAccessException("Invalid username.");

            // Compare the input password with the hashed password stored in the database
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!isPasswordValid)
                throw new UnauthorizedAccessException("Invalid password.");

            var roleIds = (await _userRoleRepository.GetListByAsync(a => a.UserId == user.UserId))
                .Select(s => s.RoleId)
                .ToArray();

                var roleMap = new Dictionary<int, string>
                {
                { 1, "Admin" },
                { 2, "Manager" }
                };

                var roles = roleIds
                .Where(id => roleMap.ContainsKey(id))
                .Select(id => roleMap[id])
                .ToList();
            var token = GenerateJwtToken(user.Username, user.Email ?? string.Empty, roles, user.UserId);
            return new LoginCommandResponse(
                user.UserId,
                user.FirstName,
                user.LastName ?? string.Empty,
                user.Email ?? string.Empty,
                user.Username,
                token
            );
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
