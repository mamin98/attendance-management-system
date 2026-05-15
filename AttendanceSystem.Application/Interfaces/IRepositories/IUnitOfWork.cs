using  AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public interface IUnitOfWork
{
    IGenericRepository<Employee> EmployeeRepository { get; }
    IGenericRepository<Department> DepartmentRepository { get; }
    IAttendanceRequestRepository AttendanceRequestRepository { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

