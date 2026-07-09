using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public static class LeaveRequestMapping
{
    public static LeaveRequestDto ToDto(this LeaveRequest entity) => new()
    {
        Id = entity.Id,
        EmployeeData = entity.Employee?.ToSimpleDto(),
        LeaveTypeData = entity.LeaveType?.ToSimpleDto(),
        StartDate = entity.StartDate,
        EndDate = entity.EndDate,
        DaysCount = entity.DaysCount,
        Reason = entity.Reason,
        Status = entity.Status.ToString()
    };
}