namespace AttendanceSystem.Application;

public class EmployeeLeaveBalanceDto
{
    public Guid Id { get; set; }
    public LeaveTypeSimpleDto? LeaveTypeData { get; set; }
    public int Year { get; set; }
    public decimal AllocatedDays { get; set; }
    public decimal UsedDays { get; set; }
    public decimal RemainingDays { get; set; }
}

public class AllocateLeaveBalanceDto
{
    public Guid EmployeeId { get; set; }
    public Guid LeaveTypeId { get; set; }
    public int Year { get; set; }
    public decimal AllocatedDays { get; set; }
}