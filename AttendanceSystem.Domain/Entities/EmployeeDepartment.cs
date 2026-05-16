namespace AttendanceSystem.Domain;

public class EmployeeDepartment : BaseEntity
{
    public Guid? EmployeeId { get; private set; }
    public Guid? DepartmentId { get; private set; }
    
    public virtual Employee? Employee { get; private set; }
    public virtual Department? Department { get; private set; }
    
    
    public EmployeeDepartment SetEmployeeId(Guid? employeeId)
    {
        EmployeeId = employeeId;
        return this;
    }

    public EmployeeDepartment SetDepartmentId(Guid? departmentId)
    {
        DepartmentId = departmentId;
        return this;
    }

    public static EmployeeDepartment Create(
        Guid? employeeId,
        Guid? departmentId) 
    => new EmployeeDepartment()
            .ApplyData(employeeId, departmentId);

    public EmployeeDepartment Update(
        Guid? employeeId,
        Guid? departmentId)       
        => ApplyData(employeeId, departmentId);
    
    private EmployeeDepartment ApplyData(
        Guid? employeeId,
        Guid? departmentId)       
    {
        SetEmployeeId(employeeId)
        .SetDepartmentId(departmentId);

        return this;
    }
}    