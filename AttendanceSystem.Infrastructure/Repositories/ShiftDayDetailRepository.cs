using AttendanceSystem.Application;
using AttendanceSystem.Domain;

namespace AttendanceSystem.Infrastructure;

public class ShiftDayDetailRepository(AttendanceDbContext context)
    : GenericRepository<ShiftDayDetail>(context), IShiftDayDetailRepository
{
    public async Task AddRangeAsync(IEnumerable<ShiftDayDetail> entities)
        => await _context.ShiftDayDetails.AddRangeAsync(entities);
}