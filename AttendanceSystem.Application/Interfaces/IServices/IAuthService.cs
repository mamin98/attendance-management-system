namespace AttendanceSystem.Application;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
    Task<LoginResponseDto> RefreshAsync(string refreshToken);
    Task LogoutAsync(string refreshToken);
    Task ChangePasswordAsync(Guid employeeId, ChangePasswordDto dto);
}