namespace SyC.Sorteo.Application.DTOs.Responses
{
    public class InscripcionDetalleResponse
    {
        public int Id { get; set; }
        public string TipoDocumento { get; set; } = string.Empty;
        public string NumeroDocumento { get; set; } = string.Empty;
        public string NombresApellidos { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public string? DocumentoUrl { get; set; }
    }
}
