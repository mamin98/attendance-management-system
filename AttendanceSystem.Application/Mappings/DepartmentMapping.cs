using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public static class DepartmentMapping
{
    public static DepartmentSimpleDto ToSimpleDto(this Department entity)
        => new()
        {
            Id = entity.Id,
            NameEnglish = entity.NameEnglish,
            NameArabic = entity.NameArabic
        };

    public static DepartmentDto ToDto(this Department entity)
        => new()
        {
            Id = entity.Id,
            NameEnglish = entity.NameEnglish,
            NameArabic = entity.NameArabic,
            ManagerData = entity.Manager?.ToSimpleDto(),
            EmployeeCount = entity.EmployeeDepartments
                .Count(ed => !ed.IsDeleted)
        };
}
