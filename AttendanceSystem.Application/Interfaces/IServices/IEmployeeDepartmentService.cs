namespace AttendanceSystem.Application;

public interface IEmployeeDepartmentService
{
    Task<List<EmployeeDepartmentDto>> GetByEmployeeIdAsync(Guid employeeId);
    Task AssignDepartmentsAsync(Guid employeeId, AssignDepartmentsDto dto);
    Task TerminateAsync(Guid employeeId, Guid departmentId, string? endDate = null);
}