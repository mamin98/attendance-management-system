using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public interface IPayrollRecordRepository : IGenericRepository<PayrollRecord>
{
    Task<PayrollRecord?> GetAsync(Guid employeeId, int year, int month);
    Task<IReadOnlyList<PayrollRecord>> GetByEmployeeIdAsync(Guid employeeId);
}