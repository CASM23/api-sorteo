using SyC.Sorteo.Application.DTOs.Requests;
using SyC.Sorteo.Application.DTOs.Responses;
using SyC.Sorteo.Domain;
using SyC.Sorteo.Domain.Enums;

namespace SyC.Sorteo.Application.Interfaces
{
    public interface IInscripcionService
    {
        Task<IEnumerable<InscripcionListItemResponse>> ListarInscripcionesAsync();
        Task<InscripcionDetalleResponse?> ObtenerDetalleAsync(int id);
        Task<bool> CambiarEstadoAsync(int id, EstadoInscripcion nuevoEstado);
        Task<InscripcionDetalleResponse> CrearInscripcionAsync(InscripcionRequest request);
    }
}
