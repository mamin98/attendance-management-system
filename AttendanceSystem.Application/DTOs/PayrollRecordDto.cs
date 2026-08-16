namespace AttendanceSystem.Application;

public class PayrollRecordDto
{
    public Guid Id { get; set; }
    public EmployeeSimpleDto? EmployeeData { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal BaseSalary { get; set; }
    public decimal LateDeductionAmount { get; set; }
    public decimal AbsenceDeductionAmount { get; set; }
    public decimal TotalDeductions { get; set; }
    public decimal NetSalary { get; set; }
}