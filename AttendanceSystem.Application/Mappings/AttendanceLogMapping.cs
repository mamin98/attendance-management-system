using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public static class AttendanceLogMapping
{
    public static AttendanceLogDto ToDto(this AttendanceLog entity) => new()
    {
        Id = entity.Id,
        EmployeeData = entity.Employee?.ToSimpleDto(),
        Date = entity.Date,
        CheckIn = entity.CheckIn,
        CheckOut = entity.CheckOut,
        Source = entity.Source
    };
}