using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public interface IHolidayRepository : IGenericRepository<Holiday>
{
    Task<PagedResult<Holiday>> GetAllWithPaginationAsync(HolidaySearchDto search);
    Task<IReadOnlyList<Holiday>> GetForPeriodAsync(DateOnly start, DateOnly end, Guid? departmentId);
}