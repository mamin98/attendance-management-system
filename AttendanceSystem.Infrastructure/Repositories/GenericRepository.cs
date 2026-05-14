using AttendanceSystem.Application;
using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Infrastructure;

public class GenericRepository<TEntity>
    : IGenericRepository<TEntity>
    where TEntity : BaseEntity
{
    protected readonly AttendanceDbContext _context;

    public GenericRepository(AttendanceDbContext context)
    {
        _context = context;
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
        => await _context.Set<TEntity>().FindAsync(id);

    public async Task<IReadOnlyList<TEntity>> GetAllAsync()
        => await _context.Set<TEntity>().ToListAsync();

    public async Task AddAsync(TEntity entity)
        => await _context.Set<TEntity>().AddAsync(entity);

    public void Update(TEntity entity)
        => _context.Set<TEntity>().Update(entity);

    public void Delete(TEntity entity)
        => entity.SoftDelete();
}