using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token);
    Task<RefreshToken?> GetByTokenAsync(string token);
    void Update(RefreshToken token);
}
