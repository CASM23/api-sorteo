namespace SyC.Sorteo.Application.DTOs.Responses
{
    public class InscripcionListItemResponse
    {
        public int Id { get; set; }
        public string NombresApellidos { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
    }
}
