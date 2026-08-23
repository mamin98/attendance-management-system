namespace AttendanceSystem.Application;

public interface IHolidayService
{
    Task<PagedResult<HolidayDto>> GetAllWithPaginationAsync(HolidaySearchDto search);
    Task<List<HolidayDto>> GetAllAsync();
    Task<HolidayDto?> GetByIdAsync(Guid id);
    Task CreateAsync(CreateHolidayDto dto);
    Task UpdateAsync(Guid id, UpdateHolidayDto dto);
    Task DeleteAsync(Guid id);
}