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
            RequestDate = entity.RequestDate.ToString(AttendanceSystemConsts.DateFormat),
            FromTime = entity.FromTime.ToString(),
            ToTime = entity.ToTime.ToString(),
            Reason = entity.Reason
        };
    }
}