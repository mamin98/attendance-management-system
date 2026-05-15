using AttendanceSystem.Application;
using AttendanceSystem.Domain;

namespace AttendanceSystem.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    readonly AttendanceDbContext _context;

    public UnitOfWork(AttendanceDbContext context) => _context = context;
    
    private IGenericRepository<Department>? _departmentRepository;
    public IGenericRepository<Department> DepartmentRepository
    {
        get
        {
            return _departmentRepository
                ??= new GenericRepository<Department>(_context);
        }
    }

    private IEmployeeRepository? _employeeRepository;
    public IEmployeeRepository EmployeeRepository
    {
        get
        {
            return _employeeRepository
                ??= new EmployeeRepository(_context);
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

    public async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    => await _context.SaveChangesAsync(cancellationToken);    
}