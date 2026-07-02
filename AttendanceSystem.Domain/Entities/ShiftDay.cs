using System.Globalization;

namespace AttendanceSystem.Domain;

public class ShiftDay : BaseEntity
{
    public DateOnly Day { get; private set; }
    public Guid ShiftId { get; private set; }
    public bool IsOffDay { get; private set; }
    public virtual Shift? Shift { get; private set; }
    public virtual ICollection<ShiftDayDetail>? ShiftDayDetails { get; private set; } = [];

    private ShiftDay SetShiftId(Guid shiftId)
    {
        ShiftId = shiftId;
        return this;
    }

    private ShiftDay SetIsOffDay(bool isOffDay)
    {
        IsOffDay = isOffDay;
        return this;
    }

    private ShiftDay SetDay(string day)
    {
        Day = DateOnly.ParseExact(day, AttendanceSystemConsts.DateFormat, CultureInfo.InvariantCulture);
        return this;
    }

    public static ShiftDay Create(Guid shiftId, bool isOffDay, string day)
        => new ShiftDay().ApplyData(shiftId, isOffDay, day);

    public ShiftDay Update(Guid shiftId, bool isOffDay, string day)
        => ApplyData(shiftId, isOffDay, day);

    private ShiftDay ApplyData(Guid shiftId, bool isOffDay, string day)
    {
        SetShiftId(shiftId).SetIsOffDay(isOffDay).SetDay(day);
        return this;
    }
}
