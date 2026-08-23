using AttendanceSystem.Application;
using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Infrastructure;

public class EmployeeLeaveBalanceRepository(AttendanceDbContext context)
    : GenericRepository<EmployeeLeaveBalance>(context), IEmployeeLeaveBalanceRepository
{
    public async Task<IReadOnlyList<EmployeeLeaveBalance>> GetByEmployeeIdAsync(Guid employeeId, int? year = null)
    {
        IQueryable<EmployeeLeaveBalance> query = _context.EmployeeLeaveBalances
            .Include(x => x.LeaveType)
            .Where(x => x.EmployeeId == employeeId);

        if (year.HasValue)
            query = query.Where(x => x.Year == year.Value);

        return await query.OrderByDescending(x => x.Year).ToListAsync();
    }

    public async Task<EmployeeLeaveBalance?> GetAsync(Guid employeeId, Guid leaveTypeId, int year)
        => await _context.EmployeeLeaveBalances
            .FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.LeaveTypeId == leaveTypeId && x.Year == year);
}