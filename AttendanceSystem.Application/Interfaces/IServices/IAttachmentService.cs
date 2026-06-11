using Microsoft.AspNetCore.Http;

namespace AttendanceSystem.Application;

public interface IAttachmentService
{
    Task<List<AttachmentDto>> GetByRequestIdAsync(Guid requestId);
    Task AddAsync(Guid requestId, IFormFile file);
    Task<(Stream stream, string fileName)> GetFileAsync(Guid attachmentId);
    Task DeleteAsync(Guid attachmentId);
}