using Microsoft.AspNetCore.Http;
using SyC.Sorteo.Domain.Interfaces;

namespace SyC.Sorteo.Infrastructure.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

        public async Task<string> SaveFileAsync(IFormFile file)
        {
            if (!Directory.Exists(_uploadPath))
                Directory.CreateDirectory(_uploadPath);

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(_uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/Uploads/{fileName}";
        }
    }
}
