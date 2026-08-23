using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public interface IEmployeeLeaveBalanceRepository : IGenericRepository<EmployeeLeaveBalance>
{
    Task<IReadOnlyList<EmployeeLeaveBalance>> GetByEmployeeIdAsync(Guid employeeId, int? year = null);
    Task<EmployeeLeaveBalance?> GetAsync(Guid employeeId, Guid leaveTypeId, int year);
}