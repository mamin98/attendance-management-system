using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public class LeaveTypeDto : LeaveTypeBaseDto
{
    public Guid Id { get; set; }
}

public class CreateLeaveTypeDto : LeaveTypeBaseDto
{
    public LeaveType ToEntity()
        => LeaveType.Create(NameEnglish, NameArabic, DefaultDaysPerYear, IsPaid, RequiresApproval);
}

public class UpdateLeaveTypeDto : CreateLeaveTypeDto
{
    public Guid Id { get; set; }

    public LeaveType UpdateEntity(LeaveType entity)
        => entity.Update(NameEnglish, NameArabic, DefaultDaysPerYear, IsPaid, RequiresApproval);
}

public class LeaveTypeSearchDto : SearchDto { }

public class LeaveTypeBaseDto
{
    public string NameEnglish { get; set; } = string.Empty;
    public string NameArabic { get; set; } = string.Empty;
    public int DefaultDaysPerYear { get; set; }
    public bool IsPaid { get; set; } = true;
    public bool RequiresApproval { get; set; } = true;
}

public class LeaveTypeSimpleDto
{
    public Guid Id { get; set; }
    public required string NameEnglish { get; set; }
    public required string NameArabic { get; set; }
}