using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public interface IEmployeeDepartmentRepository : IGenericRepository<EmployeeDepartment>
{
    Task<IReadOnlyList<EmployeeDepartment>> GetByEmployeeIdAsync(Guid employeeId);
    Task<IReadOnlyList<EmployeeDepartment>> GetByDepartmentIdsAsync(
        Guid employeeId, List<Guid> departmentIds);
    Task AddRangeAsync(IEnumerable<EmployeeDepartment> entities);
    void UpdateRange(IEnumerable<EmployeeDepartment> entities);
}