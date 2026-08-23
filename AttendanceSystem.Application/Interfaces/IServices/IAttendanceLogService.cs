namespace AttendanceSystem.Application;

public interface IAttendanceLogService
{
    Task<PagedResult<AttendanceLogDto>> GetAllWithPaginationAsync(AttendanceLogSearchDto search);
    Task<AttendanceLogDto?> GetByIdAsync(Guid id);
    Task CreateAsync(CreateAttendanceLogDto dto);
    Task UpdateAsync(Guid id, UpdateAttendanceLogDto dto);
    Task DeleteAsync(Guid id);
}