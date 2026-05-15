using Microsoft.EntityFrameworkCore;
using AttendanceSystem.Application;
using AttendanceSystem.Domain;

namespace AttendanceSystem.Infrastructure;

public class EmployeeRepository(AttendanceDbContext context)
        : GenericRepository<Employee>(context), IEmployeeRepository
{    
    public async Task<bool> IsExistAsync(Guid id)
    => await _context.Employees.AnyAsync(e => e.Id == id);
}