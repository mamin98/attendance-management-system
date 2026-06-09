using System.Security.Claims;
using AttendanceSystem.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.API;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService)
    : ControllerBase
{
    readonly IAuthService _authService = authService;

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginRequestDto dto)
    {
        LoginResponseDto result =
            await _authService.LoginAsync(dto);

        return Ok(
            ApiResponse<LoginResponseDto>
            .SuccessResponse(result));
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        Guid employeeId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await _authService.ChangePasswordAsync(employeeId, dto);

        return Ok(ApiResponse<string>.SuccessResponse(null, "Password changed successfully"));
    }

}