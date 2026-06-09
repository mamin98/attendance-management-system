namespace AttendanceSystem.Application;

public class RefreshRequestDto
{
    public string RefreshToken { get; set; } = string.Empty;
}

public class RefreshTokenValidationDto
{
    public bool IsValid { get; set; }
    public Guid EmployeeId { get; set; }
    public string Error  { get; set; } = string.Empty;
}