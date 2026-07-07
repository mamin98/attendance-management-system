namespace AttendanceSystem.Application;

public interface IShiftService
{
    Task<PagedResult<ShiftDto>> GetAllWithPaginationAsync(ShiftSearchDto search);
    Task<List<ShiftDto>> GetAllAsync();
    Task<ShiftDto?> GetByIdAsync(Guid id);
    Task CreateAsync(CreateShiftDto dto);
    Task UpdateAsync(Guid id, UpdateShiftDto dto);
    Task DeleteAsync(Guid id);
    Task<List<EmployeeShiftDto>> GetEmployeeShiftHistoryAsync(Guid employeeId);
    Task AssignShiftAsync(AssignShiftDto dto);
}