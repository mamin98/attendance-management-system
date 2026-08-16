using AttendanceSystem.Application;
using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Infrastructure;

public class SalaryStructureRepository(AttendanceDbContext context)
    : GenericRepository<SalaryStructure>(context), ISalaryStructureRepository
{
    public async Task<SalaryStructure?> GetByEmployeeIdAsync(Guid employeeId)
        => await _context.SalaryStructures.FirstOrDefaultAsync(x => x.EmployeeId == employeeId);
}