namespace AttendanceSystem.Application;

public class ShiftDayDetailDto : CreateShiftDayDetailDto
{
    public Guid Id { get; set; }
}
public class CreateShiftDayDetailDto
{
    public required string FromTime { get; set; }
    public required string ToTime { get; set; }
}
