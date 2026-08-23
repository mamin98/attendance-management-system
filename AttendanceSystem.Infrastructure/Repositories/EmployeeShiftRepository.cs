using AttendanceSystem.Application;
using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Infrastructure;
public class EmployeeShiftRepository(AttendanceDbContext context)
    : GenericRepository<EmployeeShift>(context), IEmployeeShiftRepository
{
    public async Task<IReadOnlyList<EmployeeShift>> GetByEmployeeIdAsync(Guid employeeId)
        => await _context.EmployeeShifts
            .Include(x => x.Shift)
            .Where(x => x.EmployeeId == employeeId)
            .OrderByDescending(x => x.StartDate)
            .ToListAsync();

    public async Task<EmployeeShift?> GetActiveByEmployeeIdAsync(Guid employeeId)
        => await _context.EmployeeShifts
            .Include(x => x.Shift)
            .Where(x => x.EmployeeId == employeeId && x.EndDate == null)
            .FirstOrDefaultAsync();
}