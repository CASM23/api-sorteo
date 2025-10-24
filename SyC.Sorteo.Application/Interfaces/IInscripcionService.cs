using SyC.Sorteo.Application.DTOs.Requests;
using SyC.Sorteo.Application.DTOs.Responses;

namespace SyC.Sorteo.Application.Interfaces
{
    public interface IInscripcionService
    {
        Task<InscripcionResponse> CrearInscripcionAsync(InscripcionRequest request);
    }
}
