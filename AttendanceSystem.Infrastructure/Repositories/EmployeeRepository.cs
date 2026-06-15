using System.Linq.Expressions;
using AttendanceSystem.Application;
using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Infrastructure;

public class EmployeeRepository(AttendanceDbContext context)
    : GenericRepository<Employee>(context), IEmployeeRepository
{
    public async Task<Employee?> GetByEmailAsync(string email)
        => await _context.Employees
            .FirstOrDefaultAsync(x => x.Email == email.ToLower().Trim());

    public async Task<PagedResult<Employee>> GetAllWithPaginationAsync(EmployeeSearchDto search)
    {
        Expression<Func<Employee, bool>>? filter = null;

        if (!string.IsNullOrWhiteSpace(search.Search) ||
            search.Role.HasValue ||
            search.DepartmentId.HasValue)
        {
            filter = x =>
                (string.IsNullOrWhiteSpace(search.Search) ||
                    x.NameEnglish.ToLower().Contains(search.Search.ToLower()) ||
                    x.NameArabic.ToLower().Contains(search.Search.ToLower()))
                &&
                (!search.Role.HasValue || x.Role == search.Role.Value)
                &&
                (!search.DepartmentId.HasValue ||
                    x.EmployeeDepartments.Any(ed => ed.DepartmentId == search.DepartmentId.Value));
        }

        return await GetAllWithPaginationAsync(
            search.Page,
            search.PageSize,
            filter: filter,
            include: q => q
                .Include(x => x.EmployeeDepartments)
                    .ThenInclude(ed => ed.Department),
            ignoreQueryFilters: search.IsActive.HasValue && !search.IsActive.Value
        );
    }
    public override async Task<Employee?> GetByIdAsync(Guid id)
        => await _context.Employees
            .AsNoTracking()
            .Include(x => x.EmployeeDepartments)
                .ThenInclude(ed => ed.Department)
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<bool> IsEmailUniqueAsync(string email, Guid? excludeId = null)
    {
        IQueryable<Employee> q = _context.Employees
            .Where(x => x.Email == email.ToLower().Trim());

        if (excludeId.HasValue)
            q = q.Where(x => x.Id != excludeId.Value);

        return !await q.AnyAsync();
    }
}