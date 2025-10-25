using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SyC.Sorteo.Domain.Entities;

namespace SyC.Sorteo.Infrastructure.Identity
{
    public interface IJwtService
    {
        (string Token, string Jti) GenerateToken(Usuario usuario); 
    }

    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;
        public JwtService(IConfiguration config) => _config = config;

        public (string Token, string Jti) GenerateToken(Usuario usuario) 
        {
            var jwtSection = _config.GetSection("Jwt");
            var key = jwtSection.GetValue<string>("Key")!;
            var issuer = jwtSection.GetValue<string>("Issuer");
            var audience = jwtSection.GetValue<string>("Audience");
            var expiresInMinutes = jwtSection.GetValue<int>("ExpiresIn");
            var tokenId = Guid.NewGuid().ToString(); 

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Email, usuario.Correo ?? string.Empty),
                new Claim(ClaimTypes.Role, usuario.Rol ?? "Admin"),
                new Claim(JwtRegisteredClaimNames.Jti, tokenId) 
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                signingCredentials: creds
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), tokenId);
        }
    }
}