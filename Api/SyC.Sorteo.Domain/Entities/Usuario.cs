using System;

namespace SyC.Sorteo.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }

        public string NombreUsuario { get; set; } = string.Empty;

        public string ClaveHash { get; set; } = string.Empty;

        public string Rol { get; set; } = "Admin";

        public string Correo { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }
}
