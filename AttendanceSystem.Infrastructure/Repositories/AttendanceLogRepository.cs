using AttendanceSystem.Application;
using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Infrastructure;

public class AttendanceLogRepository(AttendanceDbContext context)
    : GenericRepository<AttendanceLog>(context), IAttendanceLogRepository
{
    public async Task<AttendanceLog?> GetByEmployeeAndDateAsync(Guid employeeId, DateTime date)
        => await _context.AttendanceLogs
            .FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.Date.Date == date.Date);

    public async Task<IReadOnlyList<AttendanceLog>> GetByEmployeeAndMonthAsync(Guid employeeId, int year, int month)
        => await _context.AttendanceLogs
            .AsNoTracking()
            .Where(x => x.EmployeeId == employeeId && x.Date.Year == year && x.Date.Month == month)
            .ToListAsync();

    public async Task AddRangeAsync(IEnumerable<AttendanceLog> entities)
        => await _context.AttendanceLogs.AddRangeAsync(entities);
}