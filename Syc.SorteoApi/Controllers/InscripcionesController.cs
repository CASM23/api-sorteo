using Microsoft.AspNetCore.Mvc;
using SyC.Sorteo.Application.Interfaces;
using SyC.Sorteo.Application.DTOs.Requests;
using FluentValidation;

namespace SyC.Sorteo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InscripcionesController : ControllerBase
    {
        private readonly IInscripcionService _inscripcionService;
        private readonly IValidator<InscripcionRequest> _validator;

        public InscripcionesController(
            IInscripcionService inscripcionService,
            IValidator<InscripcionRequest> validator)
        {
            _inscripcionService = inscripcionService;
            _validator = validator;
        }

        [HttpPost]
        public async Task<IActionResult> CrearInscripcion([FromForm] InscripcionRequest request)
        {
            var validation = await _validator.ValidateAsync(request);

            if (!validation.IsValid)
                return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

            var result = await _inscripcionService.CrearInscripcionAsync(request);
            return Ok(result);
        }
    }
}
