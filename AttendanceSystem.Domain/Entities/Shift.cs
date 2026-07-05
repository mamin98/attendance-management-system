using System.Globalization;

namespace AttendanceSystem.Domain;

public class Shift : BaseEntity
{
    public string NameEnglish { get; private set; } = string.Empty;
    public string NameArabic { get; private set; } = string.Empty;
    public DateTime? StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public int GracePeriodMinutes { get; private set; }

    public virtual ICollection<ShiftDay> ShiftDays { get; private set; } = [];
    public virtual ICollection<EmployeeShift> EmployeeShifts { get; private set; } = [];
    public virtual ICollection<ShiftDayDetail>? ShiftDayDetails { get; private set; } = [];

    private Shift SetNameEnglish(string name) { NameEnglish = name.Trim(); return this; }
    private Shift SetNameArabic(string name) { NameArabic = name.Trim(); return this; }
    private Shift SetDates(string? startDate, string? endDate)
    {
        StartDate = startDate is null
            ? null
            : DateTime.ParseExact(
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
    
    private Shift SetGracePeriod(int minutes) { GracePeriodMinutes = minutes; return this; }

    public static Shift Create(string nameEn, string nameAr, string? startDate, string? endDate, int gracePeriodMinutes)
        => new Shift().ApplyData(nameEn, nameAr, startDate, endDate, gracePeriodMinutes);

    public Shift Update(string nameEn, string nameAr, string? startDate, string? endDate, int gracePeriodMinutes)
        => ApplyData(nameEn, nameAr, startDate, endDate, gracePeriodMinutes);

    private Shift ApplyData(string nameEn, string nameAr, string? startDate, string? endDate, int gracePeriodMinutes)
    {
        SetNameEnglish(nameEn).SetNameArabic(nameAr).SetDates(startDate, endDate).SetGracePeriod(gracePeriodMinutes);
        return this;
    }
}