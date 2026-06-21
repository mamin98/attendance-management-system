using  AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public interface IUnitOfWork
{
    IEmployeeRepository EmployeeRepository { get; }
    IDepartmentRepository DepartmentRepository { get; }
    IAttendanceRequestRepository AttendanceRequestRepository { get; }
    IRefreshTokenRepository RefreshTokenRepository { get; }
    IAttachmentRepository AttachmentRepository { get; }
    IGenericRepository<EmployeeDepartment> EmployeeDepartmentRepository { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

