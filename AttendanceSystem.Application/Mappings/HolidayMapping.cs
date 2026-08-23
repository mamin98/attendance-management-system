using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public static class HolidayMapping
{
    public static HolidayDto ToDto(this Holiday entity) => new()
    {
        Id = entity.Id,
        NameEnglish = entity.NameEnglish,
        NameArabic = entity.NameArabic,
        StartDate = entity.StartDate.ToString(AttendanceSystemConsts.DateFormat),
        EndDate = entity.EndDate.ToString(AttendanceSystemConsts.DateFormat),
        DepartmentData = entity.Department?.ToSimpleDto()
    };
}