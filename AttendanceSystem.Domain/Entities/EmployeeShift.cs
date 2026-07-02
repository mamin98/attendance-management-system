using System.Globalization;

namespace AttendanceSystem.Domain;

public class EmployeeShift : BaseEntity
{
    public Guid? EmployeeId { get; private set; }
    public Guid? ShiftId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }

    public virtual Employee? Employee { get; private set; }
    public virtual Shift? Shift { get; private set; }

    public EmployeeShift SetEmployeeId(Guid? employeeId) { EmployeeId = employeeId; return this; }
    public EmployeeShift SetShiftId(Guid? shiftId) { ShiftId = shiftId; return this; }

    private EmployeeShift SetDates(string startDate, string? endDate)
    {
        StartDate = DateTime.ParseExact(startDate, AttendanceSystemConsts.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None).Date;
        EndDate = endDate is null ? null : DateTime.ParseExact(endDate, AttendanceSystemConsts.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None).Date;
        return this;
    }

    public void Terminate(string? endDate = null)
    {
        EndDate = endDate is null
            ? DateTime.UtcNow.Date
            : DateTime.ParseExact(endDate, AttendanceSystemConsts.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None).Date;
    }

    public static EmployeeShift Create(Guid? employeeId, Guid? shiftId, string startDate, string? endDate = null)
        => new EmployeeShift().ApplyData(employeeId, shiftId, startDate, endDate);

    public EmployeeShift Update(Guid? employeeId, Guid? shiftId, string startDate, string? endDate = null)
        => ApplyData(employeeId, shiftId, startDate, endDate);

    private EmployeeShift ApplyData(Guid? employeeId, Guid? shiftId, string startDate, string? endDate)
    {
        SetEmployeeId(employeeId).SetShiftId(shiftId).SetDates(startDate, endDate);
        return this;
    }
}