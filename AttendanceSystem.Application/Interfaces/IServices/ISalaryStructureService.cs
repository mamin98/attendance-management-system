namespace AttendanceSystem.Application;

public interface ISalaryStructureService
{
    Task<SalaryStructureDto?> GetByEmployeeIdAsync(Guid employeeId);
    Task SetAsync(CreateSalaryStructureDto dto);
}