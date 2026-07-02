namespace AttendanceSystem.Domain;

public class ShiftDayDetail : BaseEntity
{
    public TimeOnly From { get; private set; }
    public TimeOnly To { get; private set; }
    public Guid ShiftId { get; private set; }
    public Guid ShiftDayId { get; private set; }
    public virtual Shift? Shift { get; private set; }
    public virtual ShiftDay? ShiftDay { get; private set; }

    private ShiftDayDetail SetTimes(string from, string to)
    {
        From = TimeOnly.Parse(from);
        To = TimeOnly.Parse(to);
        return this;
    }

    private ShiftDayDetail SetShiftId(Guid shiftId)
    {
        ShiftId = shiftId;
        return this;
    }

    private ShiftDayDetail SetShiftDayId(Guid shiftDayId)
    {
        ShiftDayId = shiftDayId;
        return this;
    }

    public static ShiftDayDetail Create(Guid shiftId, Guid shiftDayId, string fromTime, string toTime)
        => new ShiftDayDetail().ApplyData(shiftId, shiftDayId, fromTime, toTime);

    public ShiftDayDetail Update(Guid shiftId, Guid shiftDayId, string fromTime, string toTime)
        => ApplyData(shiftId, shiftDayId, fromTime, toTime);

    private ShiftDayDetail ApplyData(Guid shiftId, Guid shiftDayId, string fromTime, string toTime)
    {
        SetShiftId(shiftId).SetShiftId(shiftDayId).SetTimes(fromTime, toTime);
        return this;
    }
}
