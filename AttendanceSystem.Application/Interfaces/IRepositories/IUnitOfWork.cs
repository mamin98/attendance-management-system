using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public interface IUnitOfWork
{
    IEmployeeRepository EmployeeRepository { get; }
    IDepartmentRepository DepartmentRepository { get; }
    IAttendanceRequestRepository AttendanceRequestRepository { get; }
    IRefreshTokenRepository RefreshTokenRepository { get; }
    IAttachmentRepository AttachmentRepository { get; }
    IEmployeeDepartmentRepository EmployeeDepartmentRepository { get; }
    IAttendancePolicyRepository AttendancePolicyRepository { get; }
    IShiftRepository ShiftRepository { get; }
    IShiftDayRepository ShiftDayRepository { get; }
    IShiftDayDetailRepository ShiftDayDetailRepository { get; }
    IEmployeeShiftRepository EmployeeShiftRepository { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

