using AttendanceSystem.Application;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AttendanceSystem.Infrastructure;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor)
    : ICurrentUserService
{
    readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string UserId =>
        _httpContextAccessor.HttpContext?
            .User?
            .FindFirstValue(ClaimTypes.NameIdentifier)
        ?? string.Empty;

    public string UserEmail =>
        _httpContextAccessor.HttpContext?
            .User?
            .FindFirstValue(ClaimTypes.Email)
        ?? "System";

    public string UserRole =>
        _httpContextAccessor.HttpContext?
            .User?
            .FindFirstValue(ClaimTypes.Role)
        ?? string.Empty;

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?
            .User?
            .Identity?
            .IsAuthenticated
        ?? false;
}