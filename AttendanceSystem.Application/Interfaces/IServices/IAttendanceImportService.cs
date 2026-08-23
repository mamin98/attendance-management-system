using Microsoft.AspNetCore.Http;

namespace AttendanceSystem.Application;

public interface IAttendanceImportService
{
    Task<ImportAttendanceResultDto> ImportFromExcelAsync(IFormFile file);
}