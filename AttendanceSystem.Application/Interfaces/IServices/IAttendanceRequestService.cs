namespace AttendanceSystem.Application;

public interface IAttendanceRequestService
{
    
    Task<PagedResult<AttendanceRequestDto>> GetAllWithPaginationAsync(int page, int pageSize);
    Task<List<AttendanceRequestDto>> GetAllAsync();
    Task<AttendanceRequestDto?> GetByIdAsync(Guid id);
    Task CreateAsync(CreateAttendanceRequestDto dto);
    Task UpdateAsync(Guid id, UpdateAttendanceRequestDto dto);
    Task CancelAsync(Guid id);
    Task ApproveAsync(Guid id);
    Task RejectAsync(Guid id);
}