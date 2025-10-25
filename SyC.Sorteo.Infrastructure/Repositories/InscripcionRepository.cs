using Microsoft.EntityFrameworkCore;
using SyC.Sorteo.Domain.Entities;
using SyC.Sorteo.Domain.Interfaces;
using SyC.Sorteo.Infrastructure.Persistence;

namespace SyC.Sorteo.Infrastructure.Repositories
{
    public class InscripcionRepository : IInscripcionRepository
    {
        private readonly SorteoDbContext _context;

        public InscripcionRepository(SorteoDbContext context)
        {
            _context = context;
        }

        public async Task<Inscripcion> CrearAsync(Inscripcion inscripcion)
        {
            _context.Inscripciones.Add(inscripcion);
            await _context.SaveChangesAsync();
            return inscripcion;
        }

        public async Task<Inscripcion?> ObtenerPorIdAsync(int id)
        {
            return await _context.Inscripciones
                .Include(i => i.DocumentoAdjunto) 
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Inscripcion>> ObtenerTodasAsync()
        {
            return await _context.Inscripciones.ToListAsync();
        }
        public async Task UpdateAsync(Inscripcion inscripcion)
        {
            _context.Inscripciones.Update(inscripcion);
            await _context.SaveChangesAsync();
        }
    }
}
