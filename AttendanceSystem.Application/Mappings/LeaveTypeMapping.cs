using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public static class LeaveTypeMapping
{
    public static LeaveTypeSimpleDto ToSimpleDto(this LeaveType entity)
    {
        return new LeaveTypeSimpleDto
        {
            Id = entity.Id,
            NameEnglish = entity.NameEnglish,
            NameArabic = entity.NameArabic
        };
    }

    public static LeaveTypeDto ToDto(this LeaveType entity) => new()
    {
        Id = entity.Id,
        NameEnglish = entity.NameEnglish,
        NameArabic = entity.NameArabic,
        DefaultDaysPerYear = entity.DefaultDaysPerYear,
        IsPaid = entity.IsPaid,
        RequiresApproval = entity.RequiresApproval
    };
}