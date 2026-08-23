namespace AttendanceSystem.Application;

public interface IAttendanceCalculationService
{
    Task<List<MonthlyAttendanceReportDto>> GenerateMonthlyReportAsync(GenerateReportRequestDto request);
}