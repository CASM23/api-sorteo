using Microsoft.AspNetCore.Mvc;
using SyC.Sorteo.Application.Interfaces;
using SyC.Sorteo.Application.DTOs.Requests;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService) => _authService = authService;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.AuthenticateAsync(request);
        if (result == null) return Unauthorized(new { message = "Usuario o contraseña incorrectos" });
        return Ok(result);
    }
}
