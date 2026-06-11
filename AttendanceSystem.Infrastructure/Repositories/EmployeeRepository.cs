using AttendanceSystem.Application;
using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Infrastructure;

public class EmployeeRepository(AttendanceDbContext context)
    : GenericRepository<Employee>(context), IEmployeeRepository
{
    public async Task<Employee?> GetByEmailAsync(string email)
        => await _context.Employees
            .FirstOrDefaultAsync(x => x.Email == email.ToLower().Trim());
}