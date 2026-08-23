using System.Globalization;

namespace AttendanceSystem.Domain;

public class Holiday : BaseEntity
{
    public string NameEnglish { get; private set; } = string.Empty;
    public string NameArabic { get; private set; } = string.Empty;
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public Guid? DepartmentId { get; private set; }

    public virtual Department? Department { get; private set; }

    private Holiday SetNameEnglish(string name) { NameEnglish = name.Trim(); return this; }
    private Holiday SetNameArabic(string name) { NameArabic = name.Trim(); return this; }
    private Holiday SetDepartmentId(Guid? departmentId) { DepartmentId = departmentId; return this; }
    private Holiday SetDates(string startDate, string endDate)
    {
        StartDate = DateTime.ParseExact(
            startDate,
            AttendanceSystemConsts.DateFormat,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None).Date;

        EndDate = DateTime.ParseExact(
                endDate,
                AttendanceSystemConsts.DateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None).Date;

        return this;
    }

    public static Holiday Create(string nameEn, string nameAr, string startDate, string endDate, Guid? departmentId)
        => new Holiday().ApplyData(nameEn, nameAr, startDate, endDate, departmentId);

    public Holiday Update(string nameEn, string nameAr, string startDate, string endDate, Guid? departmentId)
        => ApplyData(nameEn, nameAr, startDate, endDate, departmentId);

    private Holiday ApplyData(string nameEn, string nameAr, string startDate, string endDate, Guid? departmentId)
    {
        SetNameEnglish(nameEn).SetNameArabic(nameAr).SetDates(startDate, endDate).SetDepartmentId(departmentId);
        return this;
    }
}