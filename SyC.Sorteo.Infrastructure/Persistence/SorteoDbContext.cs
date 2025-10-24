using Microsoft.EntityFrameworkCore;
using SyC.Sorteo.Domain.Entities;

namespace SyC.Sorteo.Infrastructure.Persistence
{
    public class SorteoDbContext : DbContext
    {
        public SorteoDbContext(DbContextOptions<SorteoDbContext> options)
            : base(options)
        {
        }

        // ==============================
        // DbSets (Tablas principales)
        // ==============================
        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<Inscripcion> Inscripciones { get; set; } = null!;
        public DbSet<DocumentoAdjunto> DocumentosAdjuntos { get; set; } = null!;
        public DbSet<Notificacion> Notificaciones { get; set; } = null!;

        // ==============================
        // Configuración de modelos
        // ==============================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔹 Configuración explícita de relaciones 1:1 y 1:N
            modelBuilder.Entity<Inscripcion>()
                .HasOne(i => i.DocumentoAdjunto)
                .WithOne(d => d.Inscripcion)
                .HasForeignKey<DocumentoAdjunto>(d => d.InscripcionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Inscripcion>()
                .HasMany(i => i.Notificaciones)
                .WithOne(n => n.Inscripcion)
                .HasForeignKey(n => n.InscripcionId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Índices y restricciones útiles
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.NombreUsuario)
                .IsUnique();

            modelBuilder.Entity<Inscripcion>()
                .HasIndex(i => i.NumeroDocumento);

            // 🔹 Asignación de nombres de tabla (opcional, pero ordenado)
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<Inscripcion>().ToTable("Inscripciones");
            modelBuilder.Entity<DocumentoAdjunto>().ToTable("DocumentosAdjuntos");
            modelBuilder.Entity<Notificacion>().ToTable("Notificaciones");
        }
    }
}
