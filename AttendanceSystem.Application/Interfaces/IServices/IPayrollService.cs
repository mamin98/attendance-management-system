namespace AttendanceSystem.Application;

public interface IPayrollService
{
    Task<List<PayrollRecordDto>> GeneratePayrollAsync(GenerateReportRequestDto request);
    Task<List<PayrollRecordDto>> GetEmployeeHistoryAsync(Guid employeeId);
}