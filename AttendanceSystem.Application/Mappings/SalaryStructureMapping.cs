using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public static class SalaryStructureMapping
{
    public static SalaryStructureDto ToDto(this SalaryStructure entity) => new()
    {
        Id = entity.Id,
        EmployeeId = entity.EmployeeId,
        BaseSalary = entity.BaseSalary,
        LateDeductionPerMinute = entity.LateDeductionPerMinute,
        AbsenceDeductionPerDay = entity.AbsenceDeductionPerDay
    };
}
