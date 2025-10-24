using SyC.Sorteo.Domain.Entities;

namespace SyC.Sorteo.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByNombreUsuarioAsync(string nombreUsuario);
        Task<Usuario?> GetByIdAsync(Guid id);
        Task<Usuario> CreateAsync(Usuario usuario);
        Task SaveChangesAsync();
    }
}
