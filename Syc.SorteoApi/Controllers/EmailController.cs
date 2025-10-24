using Microsoft.AspNetCore.Mvc;
using SyC.Sorteo.Domain.Interfaces; // 👈 usa la interfaz
using System.Threading.Tasks;

namespace SyC.Sorteo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet("test")]
        public async Task<IActionResult> TestEmail()
        {
            try
            {
                await _emailService.EnviarCorreoAsync(
                    "cristian11969@gmail.com",
                    "Prueba desde API .NET",
                    "<h3>Hola Casm 🚀</h3><p>Este correo fue enviado desde tu API.</p>"
                );

                return Ok("✅ Correo enviado correctamente (revisa tu inbox).");
            }
            catch (Exception ex)
            {
                return BadRequest($"❌ Error: {ex.Message}");
            }
        }
    }
}
