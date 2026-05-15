using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public class AttendanceRequestDto : AttendanceRequestBaseDto
{
    public Guid Id { get; set; }
    public EmployeeSimpleDto? EmployeeData { get; set; }
    public required string RequestType { get; set; }
    public required string RequestStatus { get; set; }
}

public class CreateAttendanceRequestDto : AttendanceRequestBaseDto
{
    public Guid EmployeeId { get; set; }
    public RequestType RequestType { get; set; }

    public AttendanceRequest ToEntity()
    {
        return AttendanceRequest.Create(
            EmployeeId,
            RequestType,
            RequestDate,
            FromTime,
            ToTime,
            Reason);
    }
}

public class UpdateAttendanceRequestDto : AttendanceRequestBaseDto
{    
   public Guid Id { get; set; }
   public RequestType RequestType { get; set; }

    public AttendanceRequest UpdateEntity(AttendanceRequest request)
   {        
        return request.Update(
            RequestType,
            RequestDate,
            FromTime,
            ToTime,
            Reason);
    }   
}


public class AttendanceRequestBaseDto
{
    public DateTime RequestDate { get; set; }
    public TimeSpan? FromTime { get; set; }
    public TimeSpan? ToTime { get; set; }
    public string? Reason { get; set; }
}