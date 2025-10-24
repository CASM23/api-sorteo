using SyC.Sorteo.Application.DTOs.Requests;
using SyC.Sorteo.Application.DTOs.Responses;

public interface IAuthService
{
    Task<LoginResponse?> AuthenticateAsync(LoginRequest request);
}
