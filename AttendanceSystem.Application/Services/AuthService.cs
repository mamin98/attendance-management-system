using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public class AuthService(
    IUnitOfWork unitOfWork,
    IJwtTokenGenerator jwtTokenGenerator,
    IRefreshTokenService refreshTokenService)
    : IAuthService
{
    readonly IUnitOfWork _unitOfWork = unitOfWork;
    readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;
    readonly IRefreshTokenService _refreshTokenService = refreshTokenService;

    public async Task<LoginResponseDto> LoginAsync(
        LoginRequestDto dto)
    {
        Employee? employee = (await _unitOfWork
            .EmployeeRepository
            .GetAllAsync())
            .FirstOrDefault(x =>
                x.Email.ToLower() == dto.Email.ToLower());

        if (employee is null || !employee.VerifyPassword(dto.Password))
            throw new UnauthorizedException("Invalid credentials");

        string token =
            _jwtTokenGenerator.GenerateToken(employee);
        string refreshToken = await _refreshTokenService.CreateAsync(employee.Id);

        return new LoginResponseDto
        {
            Token = token,
            RefreshToken = refreshToken,
            Name = employee.NameEnglish,
            Role = employee.Role.ToString()
        };
    }

    public async Task ChangePasswordAsync(Guid employeeId, ChangePasswordDto dto)
    {
        Employee? employee = await _unitOfWork.EmployeeRepository
            .GetByIdAsync(employeeId) ?? throw new NotFoundException("Employee not found");
        if (!employee.VerifyPassword(dto.CurrentPassword))
            throw new UnauthorizedException("Current password is incorrect");

        employee.UpdatePassword(dto.NewPassword);

        _unitOfWork.EmployeeRepository.Update(employee);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<LoginResponseDto> RefreshAsync(string refreshToken)
    {
        RefreshTokenValidationDto result =
            await _refreshTokenService.ValidateAsync(refreshToken);

        if (!result.IsValid)
            throw new UnauthorizedException(result.Error);

        await _refreshTokenService.RevokeAsync(refreshToken);

        Employee? employee = await _unitOfWork.EmployeeRepository
            .GetByIdAsync(result.EmployeeId) ?? throw new NotFoundException("Employee not found");

        string newAccessToken = _jwtTokenGenerator.GenerateToken(employee);
        string newRefreshToken = await _refreshTokenService.CreateAsync(employee.Id);

        return new LoginResponseDto
        {
            Token = newAccessToken,
            RefreshToken = newRefreshToken,
            Name = employee.NameEnglish,
            Role = employee.Role.ToString()
        };
    }

    public async Task LogoutAsync(string refreshToken)
    {
        await _refreshTokenService.RevokeAsync(refreshToken);
    }

}