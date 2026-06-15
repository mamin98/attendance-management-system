using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public interface IEmployeeRepository : IGenericRepository<Employee>
{
    Task<PagedResult<Employee>> GetAllWithPaginationAsync(EmployeeSearchDto search);
    Task<Employee?> GetByEmailAsync(string email);
    Task<bool> IsEmailUniqueAsync(string email, Guid? excludeId = null);
}