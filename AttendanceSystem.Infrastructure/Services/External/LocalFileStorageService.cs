using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using AttendanceSystem.Application;

namespace AttendanceSystem.Infrastructure;

public class LocalFileStorageService(IWebHostEnvironment env) : IFileStorageService
{
    public async Task<string> SaveAsync(IFormFile file, string folder)
    {            
        string uploadsPath = Path.Combine(env.ContentRootPath, "Uploads", folder);
        Directory.CreateDirectory(uploadsPath);

        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        string fullPath = Path.Combine(uploadsPath, fileName);

        using FileStream stream = new(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);

        return Path.Combine("Uploads", folder, fileName);
    }
    
    public Stream GetStream(string filePath)
    {
        if (!File.Exists(filePath))
            throw new NotFoundException("File not found on disk");

        return new FileStream(filePath, FileMode.Open, FileAccess.Read);
    }

    public void Delete(string filePath)
    {
        if (File.Exists(filePath)) File.Delete(filePath);
    }
}