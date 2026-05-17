namespace AttendanceSystem.Application;

public interface IAttendanceRequestService
{    
    Task<PagedResult<AttendanceRequestDto>> GetAllWithPaginationAsync(AttendanceRequestSearchDto search);
    Task<List<AttendanceRequestDto>> GetAllAsync();
    Task<AttendanceRequestDto?> GetByIdAsync(Guid id);
    Task<List<AttendanceRequestDto>> GetEmployeeRequestsAsync(Guid employeeId);
    Task CreateAsync(CreateAttendanceRequestDto dto);
    Task UpdateAsync(Guid id, UpdateAttendanceRequestDto dto);
    Task CancelAsync(Guid id);
    Task ApproveAsync(Guid id);
    Task RejectAsync(Guid id);
    Task DeleteAsync(Guid id);
}