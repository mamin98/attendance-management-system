using AttendanceSystem.Application;
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
}