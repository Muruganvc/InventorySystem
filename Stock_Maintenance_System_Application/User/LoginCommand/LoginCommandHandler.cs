using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Stock_Maintenance_System_Domain.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Stock_Maintenance_System_Application.User.LoginCommand
{
    internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginCommandResponse>
    {
        private readonly IRepository<Stock_Maintenance_System_Domain.User> _userRepository;
        private readonly IConfiguration _configuration;

        public LoginCommandHandler(
            IRepository<Stock_Maintenance_System_Domain.User> userRepository,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<LoginCommandResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByAsync(u => u.Username == request.UserName && u.IsActive);

            if (user is null)
                throw new UnauthorizedAccessException("Invalid username.");

            if (user.PasswordHash != request.Password)
                throw new UnauthorizedAccessException("Invalid password.");

            var token = GenerateJwtToken(user.Username, user.Email ?? string.Empty);

            return new LoginCommandResponse(
                user.UserId,
                user.FirstName,
                user.LastName ?? string.Empty,
                user.Email ?? string.Empty,
                user.Username,
                token
            );
        }
        private string GenerateJwtToken(string username, string email)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Email, email)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddMinutes(
                double.Parse(_configuration["Jwt:ExpiresInMinutes"]!));

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
