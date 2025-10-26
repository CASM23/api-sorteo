using Microsoft.AspNetCore.Mvc;
using SyC.Sorteo.Infrastructure.Persistence;
using SyC.Sorteo.Domain.Entities;

namespace SyC.SorteoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly SorteoDbContext _context;

        public UsuariosController(SorteoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var usuarios = _context.Usuarios.ToList();
            return Ok(usuarios);
        }

        [HttpPost]
        public IActionResult Create(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetAll), new { id = usuario.Id }, usuario);
        }
    }
}
