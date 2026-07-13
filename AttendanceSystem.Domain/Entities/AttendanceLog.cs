using System.Globalization;

namespace AttendanceSystem.Domain;

public class AttendanceLog : BaseEntity
{
    public Guid EmployeeId { get; private set; }
    public DateTime Date { get; private set; }
    public TimeOnly? CheckIn { get; private set; }
    public TimeOnly? CheckOut { get; private set; }
    public string Source { get; private set; } = "Import";

    public virtual Employee? Employee { get; private set; }

    private AttendanceLog SetEmployeeId(Guid employeeId) { EmployeeId = employeeId; return this; }
    private AttendanceLog SetDate(string date)
    {
        Date = DateTime.ParseExact(
            date,
            AttendanceSystemConsts.DateFormat,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None).Date;

        return this;
    }

    private AttendanceLog SetTimes(string? checkIn, string? checkOut)
    {
        CheckIn = TimeOnly.Parse(checkIn ?? string.Empty);
        CheckOut = TimeOnly.Parse(checkOut ?? string.Empty);
        return this;
    }
    private AttendanceLog SetSource(string source) { Source = source; return this; }

    public static AttendanceLog Create(Guid employeeId, string date, string? checkIn, string? checkOut, string source = "Import")
        => new AttendanceLog().ApplyData(employeeId, date, checkIn, checkOut, source);

    public AttendanceLog Update(string? checkIn, string? checkOut, string source = "Import")
        => SetTimes(checkIn, checkOut).SetSource(source);

    private AttendanceLog ApplyData(Guid employeeId, string date, string? checkIn, string? checkOut, string source)
    {
        SetEmployeeId(employeeId).SetDate(date).SetTimes(checkIn, checkOut).SetSource(source);
        return this;
    }
}