using System.Linq.Expressions;
using AttendanceSystem.Application;
using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Infrastructure;

public class AttendanceLogRepository(AttendanceDbContext context)
    : GenericRepository<AttendanceLog>(context), IAttendanceLogRepository
{
    public async Task<PagedResult<AttendanceLog>> GetAllWithPaginationAsync(AttendanceLogSearchDto search)
    {
        Expression<Func<AttendanceLog, bool>>? filter = null;

        if (search.EmployeeId.HasValue || search.FromDate.HasValue || search.ToDate.HasValue)
        {
            filter = x =>
                (!search.EmployeeId.HasValue || x.EmployeeId == search.EmployeeId)
                && (!search.FromDate.HasValue || x.Date >= search.FromDate)
                && (!search.ToDate.HasValue || x.Date <= search.ToDate);
        }

        return await GetAllWithPaginationAsync(
            search.Page,
            search.PageSize,
            filter: filter,
            include: q => q.Include(x => x.Employee));
    }

    public async Task<AttendanceLog?> GetByEmployeeAndDateAsync(Guid employeeId, DateTime date)
        => await _context.AttendanceLogs
            .FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.Date.Date == date.Date);

    public async Task<IReadOnlyList<AttendanceLog>> GetByEmployeesAndMonthAsync(List<Guid> employeeIds, int year, int month)
        => await _context.AttendanceLogs
            .AsNoTracking()
            .Where(x => employeeIds.Contains(x.EmployeeId) && x.Date.Year == year && x.Date.Month == month)
            .ToListAsync();

    public async Task AddRangeAsync(IEnumerable<AttendanceLog> entities)
        => await _context.AttendanceLogs.AddRangeAsync(entities);
    
    public override async Task<AttendanceLog?> GetByIdAsync(Guid id)
        => await _context.AttendanceLogs
            .Include(x => x.Employee)
            .FirstOrDefaultAsync(x => x.Id == id);

}