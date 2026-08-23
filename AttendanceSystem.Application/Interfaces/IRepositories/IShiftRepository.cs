using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public interface IShiftRepository : IGenericRepository<Shift>
{
    Task<PagedResult<Shift>> GetAllWithPaginationAsync(ShiftSearchDto search);
}