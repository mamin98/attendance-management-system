using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public interface IEmployeeRepository : IGenericRepository<Employee>
{
    Task<Employee?> GetByEmailAsync(string email);
}