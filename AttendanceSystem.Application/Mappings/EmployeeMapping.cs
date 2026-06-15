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

    public static EmployeeDto ToDto(this Employee entity)
        => new()
        {
            Id = entity.Id,
            NameEnglish = entity.NameEnglish,
            NameArabic = entity.NameArabic,
            Email = entity.Email,
            Role = entity.Role,
            Departments = [.. entity.EmployeeDepartments
                .Where(ed => ed.Department is not null)
                .Select(ed => ed.Department!.ToSimpleDto())]
        };
}
