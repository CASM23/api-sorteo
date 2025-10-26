using Microsoft.AspNetCore.Http;

namespace SyC.Sorteo.Application.DTOs.Requests
{
    public class InscripcionRequest
    {
        public string TipoDocumento { get; set; } = string.Empty;
        public string NumeroDocumento { get; set; } = string.Empty;
        public string NombresApellidos { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;

        public IFormFile? Documento { get; set; }
    }
}

