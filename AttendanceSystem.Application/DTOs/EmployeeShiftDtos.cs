namespace AttendanceSystem.Application;

public class EmployeeShiftDto
{
    public Guid ShiftId { get; set; }
    public required string ShiftNameEnglish { get; set; }
    public required string ShiftNameArabic { get; set; }
    public required string StartDate { get; set; }
    public string EndDate { get; set; } = string.Empty;
}

public class AssignShiftDto
{
    public Guid EmployeeId { get; set; }
    public Guid ShiftId { get; set; }
    public required string StartDate { get; set; }
    public string EndDate { get; set; } = string.Empty;

}