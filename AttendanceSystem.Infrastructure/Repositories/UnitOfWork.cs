using AttendanceSystem.Application;
using AttendanceSystem.Domain;

namespace AttendanceSystem.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    readonly AttendanceDbContext _context;

    public UnitOfWork(AttendanceDbContext context) => _context = context;

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

    private IGenericRepository<EmployeeDepartment>? _employeeDepartmentRepository;
    public IGenericRepository<EmployeeDepartment> EmployeeDepartmentRepository
    {
        get
        {
            return _employeeDepartmentRepository
                ??= new GenericRepository<EmployeeDepartment>(_context);
        }
    }

    public async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    => await _context.SaveChangesAsync(cancellationToken);
}