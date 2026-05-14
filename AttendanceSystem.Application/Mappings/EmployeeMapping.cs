using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public static class EmployeeMapping
{
    public static EmployeeSimpleDto ToSimpleDto(this Employee entity)
    {
        return new EmployeeSimpleDto
        {
            Id = entity.Id,
            NameEnglish = entity.NameEnglish,
            NameArabic = entity.NameArabic
        };
    }
}