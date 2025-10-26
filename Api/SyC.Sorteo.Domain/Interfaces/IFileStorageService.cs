using Microsoft.AspNetCore.Http;

namespace SyC.Sorteo.Domain.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(IFormFile file);
    }
}
