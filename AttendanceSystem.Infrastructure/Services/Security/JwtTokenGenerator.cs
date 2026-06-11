using AttendanceSystem.Application;
using AttendanceSystem.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AttendanceSystem.Infrastructure;

public class JwtTokenGenerator(
    IConfiguration configuration)
    : IJwtTokenGenerator
{
    readonly IConfiguration _configuration = configuration;

    public string GenerateToken(Employee employee)
    {
        IConfiguration jwtSettings = _configuration.GetSection("Jwt");

        SymmetricSecurityKey key = new(
            Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

        SigningCredentials credentials = new(
            key,
            SecurityAlgorithms.HmacSha256);

        Claim[] claims =
        [
            new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, employee.Email),
            new Claim(ClaimTypes.Role, employee.Role.ToString()),
            new Claim(ClaimTypes.Name, employee.NameEnglish)
        ];

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(jwtSettings["ExpireMinutes"])),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}