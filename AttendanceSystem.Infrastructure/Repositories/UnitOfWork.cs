using AttendanceSystem.Application;
using AttendanceSystem.Domain;

namespace AttendanceSystem.Infrastructure;

public class UnitOfWork(AttendanceDbContext context) : IUnitOfWork
{
    readonly AttendanceDbContext _context = context;

    private IEmployeeRepository? _employeeRepository;
    public IEmployeeRepository EmployeeRepository
    {
        get
        {
            return _employeeRepository
                ??= new EmployeeRepository(_context);
        }
    }

    private IDepartmentRepository? _departmentRepository;
    public IDepartmentRepository DepartmentRepository
    {
        get
        {
            return _departmentRepository
                ??= new DepartmentRepository(_context);
        }
    }

    private IAttendanceRequestRepository? _attendanceRequestRepository;
    public IAttendanceRequestRepository AttendanceRequestRepository
    {
        get
        {
            return _attendanceRequestRepository
                ??= new AttendanceRequestRepository(_context);
        }
    }

    private IRefreshTokenRepository? _refreshTokenRepository;
    public IRefreshTokenRepository RefreshTokenRepository
    {
        get
        {
            return _refreshTokenRepository
                ??= new RefreshTokenRepository(_context);
        }
    }

    private IAttachmentRepository? _attachmentRepository;
    public IAttachmentRepository AttachmentRepository
    {
        get
        {
            return _attachmentRepository
                ??= new AttachmentRepository(_context);
        }
    }

    private IEmployeeDepartmentRepository? _employeeDepartmentRepository;
    public IEmployeeDepartmentRepository EmployeeDepartmentRepository
    {
        get
        {
            return _employeeDepartmentRepository
                ??= new EmployeeDepartmentRepository(_context);
        }
    }

    private IShiftRepository? _shiftRepository;
    public IShiftRepository ShiftRepository
        => _shiftRepository ??= new ShiftRepository(_context);

    private IShiftDayRepository? _shiftDayRepository;
    public IShiftDayRepository ShiftDayRepository
        => _shiftDayRepository ??= new ShiftDayRepository(_context);

    private IShiftDayDetailRepository? _shiftDayDetailRepository;
    public IShiftDayDetailRepository ShiftDayDetailRepository
        => _shiftDayDetailRepository ??= new ShiftDayDetailRepository(_context);

    private IEmployeeShiftRepository? _employeeShiftRepository;
    public IEmployeeShiftRepository EmployeeShiftRepository
        => _employeeShiftRepository ??= new EmployeeShiftRepository(_context);

    private IAttendancePolicyRepository? _attendancePolicyRepository;
    public IAttendancePolicyRepository AttendancePolicyRepository
        => _attendancePolicyRepository ??= new AttendancePolicyRepository(_context);

    private ILeaveTypeRepository? _leaveTypeRepository;
    public ILeaveTypeRepository LeaveTypeRepository
        => _leaveTypeRepository ??= new LeaveTypeRepository(_context);

    private IEmployeeLeaveBalanceRepository? _employeeLeaveBalanceRepository;
    public IEmployeeLeaveBalanceRepository EmployeeLeaveBalanceRepository
        => _employeeLeaveBalanceRepository ??= new EmployeeLeaveBalanceRepository(_context);

    private ILeaveRequestRepository? _leaveRequestRepository;
    public ILeaveRequestRepository LeaveRequestRepository
        => _leaveRequestRepository ??= new LeaveRequestRepository(_context);

    private IAttendanceLogRepository? _attendanceLogRepository;
    public IAttendanceLogRepository AttendanceLogRepository
        => _attendanceLogRepository ??= new AttendanceLogRepository(_context);

    public async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    => await _context.SaveChangesAsync(cancellationToken);
}