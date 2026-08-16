namespace AttendanceSystem.Domain;

public class SalaryStructure : BaseEntity
{
    public Guid EmployeeId { get; private set; }
    public decimal BaseSalary { get; private set; }
    public decimal LateDeductionPerMinute { get; private set; }
    public decimal AbsenceDeductionPerDay { get; private set; }

    public virtual Employee? Employee { get; private set; }

    private SalaryStructure SetEmployeeId(Guid employeeId) { EmployeeId = employeeId; return this; }
    private SalaryStructure SetBaseSalary(decimal amount) { BaseSalary = amount; return this; }
    private SalaryStructure SetLateDeduction(decimal amount) { LateDeductionPerMinute = amount; return this; }
    private SalaryStructure SetAbsenceDeduction(decimal amount) { AbsenceDeductionPerDay = amount; return this; }

    public static SalaryStructure Create(Guid employeeId, decimal baseSalary, decimal lateDeductionPerMinute, decimal absenceDeductionPerDay)
        => new SalaryStructure().ApplyData(employeeId, baseSalary, lateDeductionPerMinute, absenceDeductionPerDay);

    public SalaryStructure Update(decimal baseSalary, decimal lateDeductionPerMinute, decimal absenceDeductionPerDay)
        => ApplyData(EmployeeId, baseSalary, lateDeductionPerMinute, absenceDeductionPerDay);

    private SalaryStructure ApplyData(Guid employeeId, decimal baseSalary, decimal lateDeductionPerMinute, decimal absenceDeductionPerDay)
    {
        SetEmployeeId(employeeId).SetBaseSalary(baseSalary).SetLateDeduction(lateDeductionPerMinute).SetAbsenceDeduction(absenceDeductionPerDay);
        return this;
    }
}