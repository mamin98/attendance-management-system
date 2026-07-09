namespace AttendanceSystem.Domain;

public class EmployeeLeaveBalance : BaseEntity
{
    public Guid EmployeeId { get; private set; }
    public Guid LeaveTypeId { get; private set; }
    public int Year { get; private set; }
    public decimal AllocatedDays { get; private set; }
    public decimal UsedDays { get; private set; }
    public decimal RemainingDays => AllocatedDays - UsedDays;

    public virtual Employee? Employee { get; private set; }
    public virtual LeaveType? LeaveType { get; private set; }

    private EmployeeLeaveBalance SetEmployeeId(Guid employeeId) { EmployeeId = employeeId; return this; }
    private EmployeeLeaveBalance SetLeaveTypeId(Guid leaveTypeId) { LeaveTypeId = leaveTypeId; return this; }
    private EmployeeLeaveBalance SetYear(int year) { Year = year; return this; }
    private EmployeeLeaveBalance SetAllocatedDays(decimal days) { AllocatedDays = days; return this; }

    public void Consume(decimal days)
    {
        if (days > RemainingDays)
            throw new InvalidOperationException("Insufficient leave balance");

        UsedDays += days;
    }

    public void Restore(decimal days) => UsedDays = Math.Max(0, UsedDays - days);

    public static EmployeeLeaveBalance Create(Guid employeeId, Guid leaveTypeId, int year, decimal allocatedDays)
        => new EmployeeLeaveBalance()
            .SetEmployeeId(employeeId)
            .SetLeaveTypeId(leaveTypeId)
            .SetYear(year)
            .SetAllocatedDays(allocatedDays);

    public EmployeeLeaveBalance AdjustAllocation(decimal allocatedDays) => SetAllocatedDays(allocatedDays);
}