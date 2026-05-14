namespace AttendanceSystem.Application;

public class EmployeeSimpleDto
{
    public Guid Id { get; set; }    
    public required string NameEnglish { get; set; }
    public required string NameArabic { get; set; }
}
