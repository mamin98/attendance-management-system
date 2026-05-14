using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;
public interface IGenericRepository<TEntity>
    where TEntity : BaseEntity
{
    Task<PagedResult<TEntity>> GetAllWithPaginationAsync(int page, int pageSize);
    Task<IReadOnlyList<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(Guid id);
    Task AddAsync(TEntity entity);

    void Update(TEntity entity);

    void Delete(TEntity entity);
}