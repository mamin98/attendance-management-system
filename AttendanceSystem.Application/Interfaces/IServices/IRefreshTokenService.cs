namespace AttendanceSystem.Application;
public interface IRefreshTokenService
{
    Task<string> CreateAsync(Guid employeeId);
    Task<RefreshTokenValidationDto> ValidateAsync(string token);
    Task RevokeAsync(string token);
}
