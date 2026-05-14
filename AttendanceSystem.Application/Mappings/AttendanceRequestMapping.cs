using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public static class AttendanceRequestMapping
{
    public static AttendanceRequestDto ToDto(this AttendanceRequest entity)
    {
        return new AttendanceRequestDto
        {
            Id = entity.Id,
            EmployeeData = entity.Employee?.ToSimpleDto(),
            RequestType = entity.RequestType.ToString(),
            RequestStatus = entity.RequestStatus.ToString(),
            RequestDate = entity.RequestDate,
            FromTime = entity.FromTime,
            ToTime = entity.ToTime,
            Reason = entity.Reason
        };
    }
}