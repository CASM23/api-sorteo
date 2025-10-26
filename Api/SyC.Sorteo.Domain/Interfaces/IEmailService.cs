namespace SyC.Sorteo.Domain.Interfaces
{
    public interface IEmailService
    {
        Task EnviarCorreoAsync(string destino, string asunto, string cuerpo);
    }
}
