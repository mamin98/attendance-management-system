using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public class AuthService(
    IUnitOfWork unitOfWork,
    IJwtTokenGenerator jwtTokenGenerator)
    : IAuthService
{
    readonly IUnitOfWork _unitOfWork = unitOfWork;
    readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;

    public async Task<LoginResponseDto> LoginAsync(
        LoginRequestDto dto)
    {
        Employee? employee = (await _unitOfWork
            .EmployeeRepository
            .GetAllAsync())
            .FirstOrDefault(x =>
                x.Email.ToLower() == dto.Email.ToLower());

        if (employee is null)
            throw new UnauthorizedException("Invalid credentials");

        string token =
            _jwtTokenGenerator.GenerateToken(employee);

        return new LoginResponseDto
        {
            Token = token,
            Name = employee.NameEnglish,
            Role = employee.Role.ToString()
        };
    }
}