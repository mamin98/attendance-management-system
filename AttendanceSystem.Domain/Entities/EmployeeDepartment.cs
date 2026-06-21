using System.Globalization;

namespace AttendanceSystem.Domain;

public class EmployeeDepartment : BaseEntity
{
    public Guid? EmployeeId { get; private set; }
    public Guid? DepartmentId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
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


    private EmployeeDepartment SetDates(string startDate, string? endDate)
    {
        StartDate = DateTime.ParseExact(
            startDate,
            AttendanceSystemConsts.DateFormat,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None).Date;

        EndDate = endDate is null
            ? null
            : DateTime.ParseExact(
                endDate,
                AttendanceSystemConsts.DateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None).Date;

        return this;
    }

    public void Terminate(string? endDate = null)
    {
        EndDate = endDate is null
            ? DateTime.UtcNow.Date
            : DateTime.ParseExact(
                endDate,
                AttendanceSystemConsts.DateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None).Date;
    }

    public static EmployeeDepartment Create(
        Guid? employeeId,
        Guid? departmentId,
        string startDate,
        string? endDate = null)
    => new EmployeeDepartment()
        .ApplyData(employeeId, departmentId, startDate, endDate);

    public EmployeeDepartment Update(
        Guid? employeeId,
        Guid? departmentId,
        string startDate,
        string? endDate = null)
    => ApplyData(employeeId, departmentId, startDate, endDate);

    private EmployeeDepartment ApplyData(
        Guid? employeeId,
        Guid? departmentId,
        string startDate,
        string? endDate)
    {
        SetEmployeeId(employeeId)
        .SetDepartmentId(departmentId)
        .SetDates(startDate, endDate);

        return this;
    }
}