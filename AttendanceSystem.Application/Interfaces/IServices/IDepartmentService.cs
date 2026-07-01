namespace AttendanceSystem.Application;

public interface IDepartmentService
{
    Task<PagedResult<DepartmentDto>> GetAllWithPaginationAsync(DepartmentSearchDto search);
    Task<List<DepartmentDto>> GetAllAsync();
    Task<DepartmentDto?> GetByIdAsync(Guid id);
    Task CreateAsync(CreateDepartmentDto dto);
    Task UpdateAsync(Guid id, UpdateDepartmentDto dto);
    Task DeleteAsync(Guid id);
}
    