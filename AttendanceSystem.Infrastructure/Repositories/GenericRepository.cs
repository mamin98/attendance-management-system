using System.Linq.Expressions;
using AttendanceSystem.Application;
using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Infrastructure;

public class GenericRepository<TEntity>
    : IGenericRepository<TEntity>
    where TEntity : BaseEntity
{
    protected readonly AttendanceDbContext _context;

    public GenericRepository(AttendanceDbContext context) => _context = context;

    public async Task<PagedResult<TEntity>> GetAllWithPaginationAsync(
        int page,
        int pageSize,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
        bool ignoreQueryFilters = false)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>().AsQueryable();

        if (ignoreQueryFilters)
            query = query.IgnoreQueryFilters();

        if (include is not null)
            query = include(query);

        if (filter is not null)
            query = query.Where(filter);

        int totalCount = await query.CountAsync();

        List<TEntity> items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        return new PagedResult<TEntity>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }
    public async Task<IReadOnlyList<TEntity>> GetAllAsync()
        => await _context.Set<TEntity>().AsNoTracking().ToListAsync();

    public async Task<IEnumerable<TEntity>> GetAllWithSearchAsync(Expression<Func<TEntity, bool>> predicate)
        => await _context.Set<TEntity>().Where(predicate).ToListAsync();


    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
        => await _context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);

    public async Task<bool> IsExistAsync(Guid id)
        => await _context.Set<TEntity>().AsNoTracking().AnyAsync(x => x.Id == id);

    public async Task AddAsync(TEntity entity)
        => await _context.Set<TEntity>().AddAsync(entity);

    public void Update(TEntity entity)
        => _context.Set<TEntity>().Update(entity);

    public void Delete(TEntity entity)
    {
        entity.SoftDelete();
        _context.Set<TEntity>().Update(entity);

    }
}