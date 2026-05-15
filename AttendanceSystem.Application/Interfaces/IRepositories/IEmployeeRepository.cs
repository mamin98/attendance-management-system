using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public interface IEmployeeRepository
    : IGenericRepository<Employee>
{
    Task<bool> IsExistAsync(Guid id);
}
