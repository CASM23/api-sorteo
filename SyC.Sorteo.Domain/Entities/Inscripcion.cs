using System;
using System.Collections.Generic;
using Syc.Sorteo.Domain.Enums;

namespace SyC.Sorteo.Domain.Entities
{
    public class Inscripcion
    {
        public int Id { get; set; }

        public string TipoDocumento { get; set; } = string.Empty;
        public string NumeroDocumento { get; set; } = string.Empty;
        public string NombresApellidos { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;

        public EstadoInscripcion Estado { get; set; } = EstadoInscripcion.Pendiente;
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        // Relaciones
        public DocumentoAdjunto? DocumentoAdjunto { get; set; }
        public ICollection<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();
    }

  
}
