using AttendanceSystem.Application;
using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Infrastructure;

public class ShiftDayRepository(AttendanceDbContext context)
    : GenericRepository<ShiftDay>(context), IShiftDayRepository
{
    public async Task<IReadOnlyList<ShiftDay>> GetByShiftIdAsync(Guid shiftId)
        => await _context.ShiftDays
            .Include(x => x.ShiftDayDetails)
            .Where(x => x.ShiftId == shiftId)
            .ToListAsync();

    public async Task AddRangeAsync(IEnumerable<ShiftDay> entities)
        => await _context.ShiftDays.AddRangeAsync(entities);

    public void RemoveRange(IEnumerable<ShiftDay> entities)
        => _context.ShiftDays.RemoveRange(entities);
}