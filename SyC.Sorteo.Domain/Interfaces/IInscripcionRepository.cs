using SyC.Sorteo.Domain.Entities;

namespace SyC.Sorteo.Domain.Interfaces
{
    public interface IInscripcionRepository
    {
        Task<Inscripcion> CrearAsync(Inscripcion inscripcion);
        Task<Inscripcion?> ObtenerPorIdAsync(Guid id);
        Task<IEnumerable<Inscripcion>> ObtenerTodasAsync();
    }
}
