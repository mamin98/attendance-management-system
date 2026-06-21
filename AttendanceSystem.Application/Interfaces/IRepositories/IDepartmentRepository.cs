using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public interface IDepartmentRepository : IGenericRepository<Department>
{
    Task<PagedResult<Department>> GetAllWithPaginationAsync(DepartmentSearchDto search);
    Task<List<Guid>> GetExistingIdsAsync(List<Guid> ids);
}