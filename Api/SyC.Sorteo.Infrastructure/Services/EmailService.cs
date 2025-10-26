using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using SyC.Sorteo.Domain.Interfaces; // 👈 agrega esto

using Microsoft.Extensions.Logging;

namespace SyC.Sorteo.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task EnviarCorreoAsync(string destino, string asunto, string cuerpo)
        {
            var host = _config["Smtp:Host"];
            var port = int.Parse(_config["Smtp:Port"]);
            var user = _config["Smtp:User"];
            var pass = _config["Smtp:Pass"];
            var from = _config["Smtp:From"];

            _logger.LogInformation("🔌 Conectando a SMTP {Host}:{Port}...", host, port);

            using var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(user, pass),
                EnableSsl = true
            };

            var message = new MailMessage(from, destino, asunto, cuerpo)
            {
                IsBodyHtml = true
            };

            try
            {
                await client.SendMailAsync(message);
                _logger.LogInformation("✅ Correo enviado correctamente a {Destino}", destino);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error al enviar correo");
                throw;
            }
        }
    }
}