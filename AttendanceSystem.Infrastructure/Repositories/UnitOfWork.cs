using AttendanceSystem.Application;

namespace AttendanceSystem.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AttendanceDbContext _context;

    public UnitOfWork(AttendanceDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    => await _context.SaveChangesAsync(cancellationToken);

}