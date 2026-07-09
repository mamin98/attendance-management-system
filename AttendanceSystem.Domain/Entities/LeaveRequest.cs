namespace AttendanceSystem.Domain;

public class LeaveRequest : BaseEntity
{
    public Guid EmployeeId { get; private set; }
    public Guid LeaveTypeId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public decimal DaysCount { get; private set; }
    public string Reason { get; private set; } = string.Empty;
    public LeaveRequestStatus Status { get; private set; } = LeaveRequestStatus.Pending;

    public virtual Employee? Employee { get; private set; }
    public virtual LeaveType? LeaveType { get; private set; }

    private LeaveRequest SetEmployeeId(Guid employeeId) { EmployeeId = employeeId; return this; }
    private LeaveRequest SetLeaveTypeId(Guid leaveTypeId) { LeaveTypeId = leaveTypeId; return this; }
    private LeaveRequest SetReason(string reason) { Reason = reason; return this; }

    private LeaveRequest SetDates(DateTime start, DateTime end)
    {
        if (end < start)
            throw new InvalidOperationException("End date cannot be before start date");

        StartDate = start.Date;
        EndDate = end.Date;
        DaysCount = (decimal)(end.Date - start.Date).TotalDays + 1;
        return this;
    }

    public LeaveRequest Approve()
    {
        if (Status != LeaveRequestStatus.Pending)
            throw new InvalidOperationException("Only pending leave requests can be approved");

        Status = LeaveRequestStatus.Approved;
        return this;
    }

    public LeaveRequest Reject()
    {
        if (Status != LeaveRequestStatus.Pending)
            throw new InvalidOperationException("Only pending leave requests can be rejected");

        Status = LeaveRequestStatus.Rejected;
        return this;
    }

    public LeaveRequest Cancel()
    {
        if (Status == LeaveRequestStatus.Cancelled)
            throw new InvalidOperationException("Leave request is already cancelled");

        Status = LeaveRequestStatus.Cancelled;
        return this;
    }

    public static LeaveRequest Create(Guid employeeId, Guid leaveTypeId, DateTime start, DateTime end, string? reason)
        => new LeaveRequest()
            .SetEmployeeId(employeeId)
            .SetLeaveTypeId(leaveTypeId)
            .SetDates(start, end)
            .SetReason(reason ?? string.Empty);

    public LeaveRequest Update(DateTime start, DateTime end, string? reason)
        => SetDates(start, end).SetReason(reason ?? string.Empty);
}