using Microsoft.AspNetCore.Http;

namespace AttendanceSystem.Application;

public interface IFileStorageService
{
    Task<string> SaveAsync(IFormFile file, string folder);
    Stream GetStream(string filePath);
    void Delete(string filePath);
}