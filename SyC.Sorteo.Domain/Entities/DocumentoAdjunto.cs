using System;

namespace SyC.Sorteo.Domain.Entities
{
    public class DocumentoAdjunto
    {
        public int Id { get; set; }

        public string NombreArchivo { get; set; } = string.Empty;
        public string TipoContenido { get; set; } = string.Empty; // image/png, application/pdf, etc.
        public string Url { get; set; } = string.Empty;

        // Relación 1:1 con Inscripcion
        public int InscripcionId { get; set; }
        public Inscripcion Inscripcion { get; set; } = null!;
    }
}
