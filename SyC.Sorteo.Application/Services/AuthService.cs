using SyC.Sorteo.Application.DTOs.Requests;
using SyC.Sorteo.Application.DTOs.Responses;
using SyC.Sorteo.Application.Interfaces;
using SyC.Sorteo.Domain.Interfaces;
using SyC.Sorteo.Infrastructure.Identity;

using Microsoft.EntityFrameworkCore; // Necesario para ToListAsync/FirstOrDefaultAsync

namespace SyC.Sorteo.Application.Services
{

    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IJwtService _jwtService;


        public AuthService(IUsuarioRepository usuarioRepo, IJwtService jwtService)
        {
            _usuarioRepo = usuarioRepo;
            _jwtService = jwtService;
          
        }

        public async Task<LoginResponse?> AuthenticateAsync(LoginRequest request)
        {
            var user = await _usuarioRepo.GetByNombreUsuarioAsync(request.NombreUsuario);
            if (user == null) return null;

            if (!PasswordHasher.Verify(request.Clave, user.ClaveHash))
                return null;

            var (token, jti) = _jwtService.GenerateToken(user);
            await _usuarioRepo.UpdateLastTokenJtiAsync(user.Id, jti);

            return new LoginResponse
            {
                Token = token,
                Expires = DateTime.UtcNow.AddMinutes(60), 
                NombreUsuario = user.NombreUsuario,
                Rol = user.Rol
            };
        }
    }
}
