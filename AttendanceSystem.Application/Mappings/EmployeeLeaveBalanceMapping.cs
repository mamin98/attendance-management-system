using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;
public static class EmployeeLeaveBalanceMapping
{
    public static EmployeeLeaveBalanceDto ToDto(this EmployeeLeaveBalance entity) => new()
    {
        Id = entity.Id,
        LeaveTypeData = entity.LeaveType?.ToSimpleDto(),
        Year = entity.Year,
        AllocatedDays = entity.AllocatedDays,
        UsedDays = entity.UsedDays,
        RemainingDays = entity.RemainingDays
    };
}