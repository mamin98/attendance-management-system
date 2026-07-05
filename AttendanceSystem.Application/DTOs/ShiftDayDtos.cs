namespace AttendanceSystem.Application;

public class ShiftDayDto : CreateShiftDayDto
{
    public Guid Id { get; set; }
}

public class CreateShiftDayDto
{
    public required string Day { get; set; }
    public bool IsOffDay { get; set; }
    public List<CreateShiftDayDetailDto> ShiftDayDetails { get; set; } = [];
}