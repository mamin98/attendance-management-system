using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public interface IShiftDayDetailRepository : IGenericRepository<ShiftDayDetail>
{
    Task AddRangeAsync(IEnumerable<ShiftDayDetail> entities);
}