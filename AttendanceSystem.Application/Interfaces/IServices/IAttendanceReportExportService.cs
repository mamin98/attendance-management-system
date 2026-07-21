using Microsoft.AspNetCore.Http;

namespace AttendanceSystem.Application;

public interface IAttendanceReportExportService
{
    Task<byte[]> ExportToExcelAsync(List<MonthlyAttendanceReportDto> reports);
}