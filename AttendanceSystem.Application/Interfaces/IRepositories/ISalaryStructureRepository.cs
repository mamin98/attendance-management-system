using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public interface ISalaryStructureRepository : IGenericRepository<SalaryStructure>
{
    Task<SalaryStructure?> GetByEmployeeIdAsync(Guid employeeId);
}