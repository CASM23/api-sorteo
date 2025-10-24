using SyC.Sorteo.Application.DTOs.Requests;
using SyC.Sorteo.Application.DTOs.Responses;
using SyC.Sorteo.Domain.Entities;
using SyC.Sorteo.Domain.Interfaces;
using SyC.Sorteo.Application.Interfaces;
using Syc.Sorteo.Domain.Enums;

namespace SyC.Sorteo.Application.Services
{
    public class InscripcionService : IInscripcionService
    {
        private readonly IInscripcionRepository _inscripcionRepository;
        private readonly IFileStorageService _fileStorageService;

        public InscripcionService(IInscripcionRepository inscripcionRepository, IFileStorageService fileStorageService)
        {
            _inscripcionRepository = inscripcionRepository;
            _fileStorageService = fileStorageService;
        }

        public async Task<InscripcionResponse> CrearInscripcionAsync(InscripcionRequest request)
        {
            string? fileUrl = null;

            // 🔹 Guardar archivo si se adjunta
            if (request.Documento != null)
            {
                fileUrl = await _fileStorageService.SaveFileAsync(request.Documento);
            }

            var inscripcion = new Inscripcion
            {
                TipoDocumento = request.TipoDocumento,
                NumeroDocumento = request.NumeroDocumento,
                NombresApellidos = request.NombresApellidos,
                FechaNacimiento = request.FechaNacimiento,
                Direccion = request.Direccion,
                Telefono = request.Telefono,
                Correo = request.Correo,
                FechaRegistro = DateTime.UtcNow,
                Estado = EstadoInscripcion.Pendiente,
                DocumentoAdjunto = fileUrl != null ? new DocumentoAdjunto
                {
                    NombreArchivo = request.Documento!.FileName,
                    Url = fileUrl,
                    TipoContenido = request.Documento.ContentType
                } : null
            };

            await _inscripcionRepository.CrearAsync(inscripcion);
            
            return new InscripcionResponse
            {
                Id = inscripcion.Id,
                NombresApellidos = inscripcion.NombresApellidos,
                Estado = inscripcion.Estado.ToString(),
                FechaRegistro = inscripcion.FechaRegistro
            };
        }
    }
}
