using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public class AttendancePolicyDto : AttendancePolicyBaseDto
{
    public Guid Id { get; set; }
}

public class CreateAttendancePolicyDto : AttendancePolicyBaseDto
{
    public AttendancePolicy ToEntity()
        => AttendancePolicy.Create(NameEnglish, NameArabic, MaxLateMinutesPerMonth, MaxPermissionsPerMonth, MaxRemoteDaysPerMonth, MaxEarlyLeavesPerMonth, RequiresManagerApproval);
}

public class UpdateAttendancePolicyDto : AttendancePolicyBaseDto
{
    public Guid Id { get; set; }
    public AttendancePolicy UpdateEntity(AttendancePolicy entity)
        => entity.Update(NameEnglish, NameArabic, MaxLateMinutesPerMonth, MaxPermissionsPerMonth, MaxRemoteDaysPerMonth, MaxEarlyLeavesPerMonth, RequiresManagerApproval);
}

public class AttendancePolicySearchDto : SearchDto { }

public class AssignPolicyToDepartmentDto
{
    public Guid DepartmentId { get; set; }
    public Guid PolicyId { get; set; }
}


public class AttendancePolicyBaseDto
{
    public required string NameEnglish { get; set; }
    public required string NameArabic { get; set; }
    public int MaxLateMinutesPerMonth { get; set; }
    public int MaxPermissionsPerMonth { get; set; }
    public int MaxRemoteDaysPerMonth { get; set; }
    public int MaxEarlyLeavesPerMonth { get; set; }
    public bool RequiresManagerApproval { get; set; }
    public int DepartmentCount { get; set; }
}