using AttendanceSystem.Application;
using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Infrastructure;

public class EmployeeDepartmentRepository
    : GenericRepository<EmployeeDepartment>, IEmployeeDepartmentRepository
{
    public EmployeeDepartmentRepository(AttendanceDbContext context)
        : base(context)
    {}

    public async Task<IReadOnlyList<EmployeeDepartment>> GetByEmployeeIdAsync(Guid employeeId)
        => await _context.EmployeeDepartments
            .Where(x => x.EmployeeId == employeeId)
            .ToListAsync();

    public async Task<IReadOnlyList<EmployeeDepartment>> GetByDepartmentIdsAsync(
        Guid employeeId, List<Guid> departmentIds)
        => await _context.EmployeeDepartments
            .Where(x => x.EmployeeId == employeeId
                && departmentIds.Contains(x.DepartmentId!.Value))
            .ToListAsync();

    public async Task AddRangeAsync(IEnumerable<EmployeeDepartment> entities)
        => await _context.EmployeeDepartments.AddRangeAsync(entities);

    public void UpdateRange(IEnumerable<EmployeeDepartment> entities)
        => _context.EmployeeDepartments.UpdateRange(entities);

 }