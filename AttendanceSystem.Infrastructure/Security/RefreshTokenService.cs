using AttendanceSystem.Application;
using AttendanceSystem.Domain;

namespace AttendanceSystem.Infrastructure;

public class RefreshTokenService(IUnitOfWork unitOfWork)
    : IRefreshTokenService
{
    readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<string> CreateAsync(Guid employeeId)
    {
        RefreshToken token = RefreshToken.Create(employeeId, RefreshTokenExpiry.SevenDays);
        await _unitOfWork.RefreshTokenRepository.AddAsync(token);
        
        return token.Token;
    }

    public async Task<RefreshTokenValidationDto> ValidateAsync(string token)
    {
        RefreshToken? existing = await _unitOfWork.RefreshTokenRepository.GetByTokenAsync(token);

        if (existing is null)
            return new() { IsValid = false, Error = "Invalid refresh token" };

        if (!existing.IsActive)
            return new() { IsValid = false, Error = "Refresh token has expired or been revoked" };

        return new() { IsValid = true, EmployeeId = existing.EmployeeId };
    }

    public async Task RevokeAsync(string token)
    {
        RefreshToken? existing = await _unitOfWork.RefreshTokenRepository.GetByTokenAsync(token);
        if (existing is null) return;
       
        existing.Revoke();
        _unitOfWork.RefreshTokenRepository.Update(existing);
    }
}
