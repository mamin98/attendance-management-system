namespace AttendanceSystem.Application;

public interface ILeaveTypeService
{
    Task<PagedResult<LeaveTypeDto>> GetAllWithPaginationAsync(LeaveTypeSearchDto search);
    Task<List<LeaveTypeDto>> GetAllAsync();
    Task<LeaveTypeDto?> GetByIdAsync(Guid id);
    Task CreateAsync(CreateLeaveTypeDto dto);
    Task UpdateAsync(Guid id, UpdateLeaveTypeDto dto);
    Task DeleteAsync(Guid id);
    Task<List<EmployeeLeaveBalanceDto>> GetEmployeeBalancesAsync(Guid employeeId, int? year = null);
    Task AllocateBalanceAsync(AllocateLeaveBalanceDto dto);
}