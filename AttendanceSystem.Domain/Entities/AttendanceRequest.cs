using AttendanceSystem.Domain.Common;
using AttendanceSystem.Domain.Enums;
namespace AttendanceSystem.Domain.Entities;

public class AttendanceRequest : BaseEntity
{
    public Guid EmployeeId { get; private set; }
    public RequestType RequestType { get; private set; }
    public RequestStatus RequestStatus { get; private set; } = RequestStatus.Pending;
    public DateTime RequestDate { get; private set; }
    public TimeSpan? FromTime { get; private set; }
    public TimeSpan? ToTime { get; private set; }
    public string Reason { get; private set; } = string.Empty;
    public virtual Employee? Employee { get; private set; }

    private AttendanceRequest SetEmployeeId(Guid employeeId)
    {
        EmployeeId = employeeId;
        return this;
    }

    private AttendanceRequest SetRequestType(RequestType type)
    {
        RequestType = type;
        return this;
    }

    public AttendanceRequest Approve()
    {
        if (RequestStatus != RequestStatus.Pending)
            throw new InvalidOperationException("Only pending requests can be approved");

        RequestStatus = RequestStatus.Approved;
        return this;
    }

    public AttendanceRequest Reject()
    {
        if (RequestStatus != RequestStatus.Pending)
            throw new InvalidOperationException("Only pending requests can be rejected");

        RequestStatus = RequestStatus.Rejected;
        return this;
    }

    public AttendanceRequest Cancel()
    {
        if (RequestStatus != RequestStatus.Pending)
            throw new InvalidOperationException("Only pending requests can be cancelled");

        RequestStatus = RequestStatus.Cancelled;
        return this;
    }

    private AttendanceRequest SetDate(DateTime date)
    {
        RequestDate = date;
        return this;
    }

    private AttendanceRequest SetTime(TimeSpan? from, TimeSpan? to)
    {
        if (from.HasValue && to.HasValue && from > to)
            throw new ArgumentException("FromTime cannot be greater than ToTime");

        FromTime = from;
        ToTime = to;

        return this;
    }

    private AttendanceRequest SetReason(string reason)
    {
        Reason = reason;
        return this;
    }


    public static AttendanceRequest Create(
     Guid employeeId,
     RequestType type,
     DateTime date,
     TimeSpan? from,
     TimeSpan? to,
     string? reason = null)
     => new AttendanceRequest()
         .ApplyData(employeeId, type, date, from, to, reason);

    public AttendanceRequest Update(
     DateTime date,
     TimeSpan? from,
     TimeSpan? to,
     string? reason)
     => ApplyData(EmployeeId, RequestType, date, from, to, reason);

    private AttendanceRequest ApplyData(
        Guid employeeId,
        RequestType type,
        DateTime date,
        TimeSpan? from,
        TimeSpan? to,
        string? reason)
    {
        SetEmployeeId(employeeId)
            .SetRequestType(type)
            .SetDate(date)
            .SetTime(from, to)
            .SetReason(reason ?? string.Empty);

        return this;
    }

}
