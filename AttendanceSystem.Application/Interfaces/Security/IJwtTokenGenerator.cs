using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public interface IJwtTokenGenerator
{
    string GenerateToken(Employee employee);
}