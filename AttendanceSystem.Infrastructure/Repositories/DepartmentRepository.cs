using System.Linq.Expressions;
using AttendanceSystem.Application;
using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Infrastructure;

public class DepartmentRepository(AttendanceDbContext context)
    : GenericRepository<Department>(context), IDepartmentRepository
{
    public async Task<PagedResult<Department>> GetAllWithPaginationAsync(DepartmentSearchDto search)
    {
        Expression<Func<Department, bool>>? filter = null;

        if (!string.IsNullOrWhiteSpace(search.Search) ||
            search.ManagerId.HasValue)
        {
            filter = x =>
                (string.IsNullOrWhiteSpace(search.Search) ||
                    x.NameEnglish.ToLower().Contains(search.Search.ToLower()) ||
                    x.NameArabic.ToLower().Contains(search.Search.ToLower()))
                &&
                (!search.ManagerId.HasValue || x.ManagerId == search.ManagerId.Value);
        }

        return await GetAllWithPaginationAsync(
            search.Page,
            search.PageSize,
            filter: filter,
            include: q => q
                .Include(x => x.Manager)
                .Include(x => x.EmployeeDepartments));
    }

    public override async Task<Department?> GetByIdAsync(Guid id)
        => await _context.Departments
            .AsNoTracking()
            .Include(x => x.Manager)
            .Include(x => x.EmployeeDepartments)            
            .FirstOrDefaultAsync(x => x.Id == id);

}