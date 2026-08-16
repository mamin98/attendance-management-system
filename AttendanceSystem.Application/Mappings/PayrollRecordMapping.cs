using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public static class PayrollRecordMapping
{
    public static PayrollRecordDto ToDto(this PayrollRecord entity) => new()
    {
        EmployeeData = entity.Employee?.ToSimpleDto(),
        Month = entity.Month,
        Year = entity.Year,
        BaseSalary = entity.BaseSalary,
        LateDeductionAmount = entity.LateDeductionAmount,
        AbsenceDeductionAmount = entity.AbsenceDeductionAmount,
        TotalDeductions = entity.TotalDeductions,
        NetSalary = entity.NetSalary
    };
}
