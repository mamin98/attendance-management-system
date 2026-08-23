namespace AttendanceSystem.Application;

public interface ILeaveRequestService
{
    Task<PagedResult<LeaveRequestDto>> GetAllWithPaginationAsync(LeaveRequestSearchDto search);
    Task<List<LeaveRequestDto>> GetAllAsync();
    Task<LeaveRequestDto?> GetByIdAsync(Guid id);
    Task<List<LeaveRequestDto>> GetEmployeeRequestsAsync(Guid employeeId);
    Task CreateAsync(CreateLeaveRequestDto dto);
    Task UpdateAsync(Guid id, UpdateLeaveRequestDto dto);
    Task ApproveAsync(Guid id);
    Task RejectAsync(Guid id);
    Task CancelAsync(Guid id);
    Task DeleteAsync(Guid id);
}