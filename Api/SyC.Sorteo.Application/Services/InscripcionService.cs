using SyC.Sorteo.Application.DTOs.Requests;
using SyC.Sorteo.Application.DTOs.Responses;
using SyC.Sorteo.Application.Interfaces;
using SyC.Sorteo.Domain.Entities;
using SyC.Sorteo.Domain;
using SyC.Sorteo.Domain.Enums;
using SyC.Sorteo.Domain.Interfaces;

namespace SyC.Sorteo.Application.Services
{
    public class InscripcionService : IInscripcionService
    {
        private readonly IInscripcionRepository _repo;
        private readonly IFileStorageService _fileStorage;
        private readonly IEmailService _emailService;

        public InscripcionService(
            IInscripcionRepository repo,
            IFileStorageService fileStorage,
            IEmailService emailService)
        {
            _repo = repo;
            _fileStorage = fileStorage;
            _emailService = emailService;
        }

        public async Task<InscripcionResponse> CrearInscripcionAsync(InscripcionRequest request)
        {
            string filePath = string.Empty;
            if (request.Documento != null)
                filePath = await _fileStorage.SaveFileAsync(request.Documento);

            var insc = new Inscripcion
            {
                TipoDocumento = request.TipoDocumento,
                NumeroDocumento = request.NumeroDocumento,
                NombresApellidos = request.NombresApellidos,
                FechaNacimiento = request.FechaNacimiento,
                Direccion = request.Direccion,
                Telefono = request.Telefono,
                Correo = request.Correo,
                Estado = EstadoInscripcion.Pendiente,
                FechaRegistro = DateTime.UtcNow,
                DocumentoAdjunto = new DocumentoAdjunto
                {
                    NombreArchivo = Path.GetFileName(filePath),
                    Url = filePath,
                    TipoContenido = request.Documento?.ContentType ?? "application/pdf"
                }
            };

            var created = await _repo.CrearAsync(insc);

            return new InscripcionResponse
            {
                Id = created.Id,
                NombresApellidos = insc.NombresApellidos,
                FechaRegistro = insc.FechaRegistro,
                Estado = insc.Estado.ToString()
            };
        }

        public async Task<IEnumerable<InscripcionListItemResponse>> ListarInscripcionesAsync()
        {
            var list = await _repo.ObtenerTodasAsync();
            return list.Select(i => new InscripcionListItemResponse
            {
                Id = i.Id,
                NombresApellidos = i.NombresApellidos,
                FechaRegistro = i.FechaRegistro
            });
        }

        public async Task<InscripcionDetalleResponse?> ObtenerDetalleAsync(int id)
        {
            var i = await _repo.ObtenerPorIdAsync(id);
            if (i == null) return null;

            return new InscripcionDetalleResponse
            {
                Id = i.Id,
                TipoDocumento = i.TipoDocumento,
                NumeroDocumento = i.NumeroDocumento,
                NombresApellidos = i.NombresApellidos,
                FechaNacimiento = i.FechaNacimiento,
                Direccion = i.Direccion,
                Telefono = i.Telefono,
                Correo = i.Correo,
                Estado = i.Estado.ToString(),
                FechaRegistro = i.FechaRegistro,
                DocumentoUrl = i.DocumentoAdjunto?.Url
            };
        }

        public async Task<bool> CambiarEstadoAsync(int id, EstadoInscripcion nuevoEstado)
        {
            var insc = await _repo.ObtenerPorIdAsync(id);
            if (insc == null) return false;

            insc.Estado = nuevoEstado;
            await _repo.UpdateAsync(insc);

            string mensaje = nuevoEstado == EstadoInscripcion.Aceptada
                ? "Su inscripción ha sido aceptada."
                : "Su inscripción ha sido rechazada.";

            await _emailService.EnviarCorreoAsync(insc.Correo, "Resultado de su inscripción", mensaje);
            return true;
        }
    }
}
