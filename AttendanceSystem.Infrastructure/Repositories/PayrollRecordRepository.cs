using AttendanceSystem.Application;
using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Infrastructure;

public class PayrollRecordRepository(AttendanceDbContext context)
    : GenericRepository<PayrollRecord>(context), IPayrollRecordRepository
{
    public async Task<PayrollRecord?> GetAsync(Guid employeeId, int year, int month)
        => await _context.PayrollRecords
            .FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.Year == year && x.Month == month);

    public async Task<IReadOnlyList<PayrollRecord>> GetByEmployeeIdAsync(Guid employeeId)
        => await _context.PayrollRecords
            .Include(x => x.Employee)
            .Where(x => x.EmployeeId == employeeId)
            .OrderByDescending(x => x.Year).ThenByDescending(x => x.Month)
            .ToListAsync();
}