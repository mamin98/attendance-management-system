using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public interface IEmployeeShiftRepository : IGenericRepository<EmployeeShift>
{
    Task<IReadOnlyList<EmployeeShift>> GetByEmployeeIdAsync(Guid employeeId);
    Task<EmployeeShift?> GetActiveByEmployeeIdAsync(Guid employeeId);
}