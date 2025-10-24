namespace SyC.Sorteo.Application.DTOs.Responses
{
    public class InscripcionResponse
    {
        public int Id { get; set; }
        public string NombresApellidos { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
    }
}
