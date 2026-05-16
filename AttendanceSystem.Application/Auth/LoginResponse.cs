namespace AttendanceSystem.Application;

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
}