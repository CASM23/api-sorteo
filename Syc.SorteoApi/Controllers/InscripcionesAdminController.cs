using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyC.Sorteo.Application.DTOs.Requests;
using SyC.Sorteo.Application.Interfaces;
using SyC.Sorteo.Domain.Enums;

namespace SyC.Sorteo.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/admin/inscripciones")]
    public class InscripcionesAdminController : ControllerBase
    {
        private readonly IInscripcionService _service;

        public InscripcionesAdminController(IInscripcionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var result = await _service.ListarInscripcionesAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerDetalle(int id)
        {
            var result = await _service.ObtenerDetalleAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("{id:int}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] CambioEstadoRequest request)
        {
            if (!Enum.TryParse<EstadoInscripcion>(request.Estado, true, out var nuevoEstado))
                return BadRequest("Estado inválido. Use 'Aceptada' o 'Rechazada'.");

            var ok = await _service.CambiarEstadoAsync(id, nuevoEstado);
            if (!ok)
                return NotFound();

            return Ok(new { mensaje = "✅ Estado actualizado correctamente" });

        }
    }
}
