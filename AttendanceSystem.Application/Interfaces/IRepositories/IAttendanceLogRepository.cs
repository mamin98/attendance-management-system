using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public interface IAttendanceLogRepository : IGenericRepository<AttendanceLog>
{
    Task<AttendanceLog?> GetByEmployeeAndDateAsync(Guid employeeId, DateTime date);
    Task<IReadOnlyList<AttendanceLog>> GetByEmployeesAndMonthAsync(List<Guid> employeeId, int year, int month);
    Task AddRangeAsync(IEnumerable<AttendanceLog> entities);
}