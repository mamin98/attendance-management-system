using AttendanceSystem.Application;
using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Infrastructure;

public class RefreshTokenRepository(AttendanceDbContext context)
    : IRefreshTokenRepository
{
    public async Task AddAsync(RefreshToken token)
        => await context.RefreshTokens.AddAsync(token);

    public async Task<RefreshToken?> GetByTokenAsync(string token)
        => await context.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == token && !x.IsDeleted);

    public void Update(RefreshToken token)
        => context.RefreshTokens.Update(token);
}
