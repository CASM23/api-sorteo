using Microsoft.EntityFrameworkCore;
using SyC.Sorteo.Domain.Entities;
using SyC.Sorteo.Domain.Interfaces;
using SyC.Sorteo.Infrastructure.Persistence;

namespace SyC.Sorteo.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly SorteoDbContext _context;

        public UsuarioRepository(SorteoDbContext context) => _context = context;

        public async Task<Usuario?> GetByNombreUsuarioAsync(string nombreUsuario) =>
            await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);

        public async Task<Usuario?> GetByIdAsync(int id) =>
            await _context.Usuarios.FindAsync(id);

        public async Task<Usuario> CreateAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task UpdateLastTokenJtiAsync(int userId, string jti)
        {
          
            var lastToken = await _context.LastTokenIds
                .FirstOrDefaultAsync(t => t.UsuarioId == userId);

            if (lastToken == null)
            {
             
                _context.LastTokenIds.Add(new LastTokenId { UsuarioId = userId, Jti = jti });
            }
            else
            {
                
                lastToken.Jti = jti;
            }

            await _context.SaveChangesAsync();
        }




    }
}
