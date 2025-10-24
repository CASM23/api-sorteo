using SyC.Sorteo.Application.DTOs.Requests;
using SyC.Sorteo.Application.DTOs.Responses;
using SyC.Sorteo.Application.Interfaces;
using SyC.Sorteo.Domain.Interfaces;
using SyC.Sorteo.Infrastructure.Identity;

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

            var token = _jwtService.GenerateToken(user);
            return new LoginResponse
            {
                Token = token,
                Expires = DateTime.UtcNow.AddMinutes(60), // o leer de config
                NombreUsuario = user.NombreUsuario,
                Rol = user.Rol
            };
        }
    }
}
