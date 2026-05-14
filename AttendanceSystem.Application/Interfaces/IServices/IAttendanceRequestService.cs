namespace AttendanceSystem.Application;

public interface IAttendanceRequestService
{
    Task<Guid> CreateAsync(CreateAttendanceRequestDto dto);
    Task<List<AttendanceRequestDto>> GetAllAsync();
    Task<AttendanceRequestDto?> GetByIdAsync(Guid id);
    Task UpdateAsync(Guid id, UpdateAttendanceRequestDto dto);
    Task CancelAsync(Guid id);
    Task ApproveAsync(Guid id);
    Task RejectAsync(Guid id);
}