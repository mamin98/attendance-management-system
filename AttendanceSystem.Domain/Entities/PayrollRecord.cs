namespace AttendanceSystem.Domain;

public class PayrollRecord : BaseEntity
{
    public Guid EmployeeId { get; private set; }
    public int Month { get; private set; }
    public int Year { get; private set; }
    public decimal BaseSalary { get; private set; }
    public decimal LateDeductionAmount { get; private set; }
    public decimal AbsenceDeductionAmount { get; private set; }
    public decimal TotalDeductions { get; private set; }
    public decimal NetSalary { get; private set; }

    public virtual Employee? Employee { get; private set; }

    public static PayrollRecord Create(
        Guid employeeId, int month, int year,
        decimal baseSalary, decimal lateDeductionAmount, decimal absenceDeductionAmount)
    {
        decimal totalDeductions = lateDeductionAmount + absenceDeductionAmount;

        return new PayrollRecord
        {
            EmployeeId = employeeId,
            Month = month,
            Year = year,
            BaseSalary = baseSalary,
            LateDeductionAmount = lateDeductionAmount,
            AbsenceDeductionAmount = absenceDeductionAmount,
            TotalDeductions = totalDeductions,
            NetSalary = Math.Max(0, baseSalary - totalDeductions)
        };
    }
}