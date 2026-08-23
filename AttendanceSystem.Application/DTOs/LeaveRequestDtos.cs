using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public class LeaveRequestDto
{
    public Guid Id { get; set; }
    public EmployeeSimpleDto? EmployeeData { get; set; }
    public LeaveTypeSimpleDto? LeaveTypeData { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal DaysCount { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class CreateLeaveRequestDto
{
    public Guid EmployeeId { get; set; }
    public Guid LeaveTypeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Reason { get; set; }
}

public class UpdateLeaveRequestDto
{
    public Guid Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Reason { get; set; }
}

public class LeaveRequestSearchDto : SearchDto
{
    public Guid? EmployeeId { get; set; }
    public Guid? LeaveTypeId { get; set; }
    public LeaveRequestStatus? Status { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}