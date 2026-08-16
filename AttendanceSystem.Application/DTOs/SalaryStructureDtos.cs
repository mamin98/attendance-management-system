namespace AttendanceSystem.Application;

public class SalaryStructureDto : SalaryStructureBaseDto
{
    public Guid Id { get; set; }
}

public class CreateSalaryStructureDto : SalaryStructureBaseDto { }

public class SalaryStructureBaseDto
{
    public Guid EmployeeId { get; set; }
    public decimal BaseSalary { get; set; }
    public decimal LateDeductionPerMinute { get; set; }
    public decimal AbsenceDeductionPerDay { get; set; }
}