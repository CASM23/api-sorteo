using System;

namespace SyC.Sorteo.Domain.Entities
{
    public class Notificacion
    {
        public int Id { get; set; }

        public int InscripcionId { get; set; }
        public Inscripcion Inscripcion { get; set; } = null!;

        public string Medio { get; set; } = "Email"; 
        public string Destinatario { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;

        public bool Enviado { get; set; } = false;
        public DateTime FechaEnvio { get; set; } = DateTime.UtcNow;
    }
}
