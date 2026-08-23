using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public interface IShiftDayRepository : IGenericRepository<ShiftDay>
{
    Task<IReadOnlyList<ShiftDay>> GetByShiftIdAsync(Guid shiftId);
    Task AddRangeAsync(IEnumerable<ShiftDay> entities);
    void RemoveRange(IEnumerable<ShiftDay> entities);
}
