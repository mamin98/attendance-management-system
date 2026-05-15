using  AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public interface IUnitOfWork
{
    IGenericRepository<Department> DepartmentRepository { get; }
    IEmployeeRepository EmployeeRepository { get; }
    IAttendanceRequestRepository AttendanceRequestRepository { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

