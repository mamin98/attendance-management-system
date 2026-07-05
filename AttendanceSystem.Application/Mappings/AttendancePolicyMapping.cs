using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public static class AttendancePolicyMapping
{
    public static AttendancePolicyDto ToDto(this AttendancePolicy entity) => new()
    {
        Id = entity.Id,
        NameEnglish = entity.NameEnglish,
        NameArabic = entity.NameArabic,
        MaxLateMinutesPerMonth = entity.MaxLateMinutesPerMonth,
        MaxPermissionsPerMonth = entity.MaxPermissionsPerMonth,
        MaxRemoteDaysPerMonth = entity.MaxRemoteDaysPerMonth,
        MaxEarlyLeavesPerMonth = entity.MaxEarlyLeavesPerMonth,
        RequiresManagerApproval = entity.RequiresManagerApproval,
        DepartmentCount = entity.Departments.Count(d => !d.IsDeleted)
    };
}