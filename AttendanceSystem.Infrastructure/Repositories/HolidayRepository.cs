using System.Linq.Expressions;
using AttendanceSystem.Application;
using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Infrastructure;

public class HolidayRepository(AttendanceDbContext context)
    : GenericRepository<Holiday>(context), IHolidayRepository
{
    public async Task<PagedResult<Holiday>> GetAllWithPaginationAsync(HolidaySearchDto search)
    {
        Expression<Func<Holiday, bool>>? filter = null;

        if (!string.IsNullOrWhiteSpace(search.Search) || search.DepartmentId.HasValue)
        {
            filter = x =>
                (string.IsNullOrWhiteSpace(search.Search) ||
                    x.NameEnglish.ToLower().Contains(search.Search.ToLower()) ||
                    x.NameArabic.ToLower().Contains(search.Search.ToLower()))
                && (!search.DepartmentId.HasValue || x.DepartmentId == search.DepartmentId);
        }

        return await GetAllWithPaginationAsync(search.Page, search.PageSize, filter: filter);
    }

    public async Task<IReadOnlyList<Holiday>> GetForPeriodAsync(DateOnly start, DateOnly end, Guid? departmentId)
        => await _context.Holidays
            .AsNoTracking()
            .Where(x => DateOnly.FromDateTime(x.StartDate) <= end && DateOnly.FromDateTime(x.EndDate) >= start
                && (x.DepartmentId == null || x.DepartmentId == departmentId))
            .ToListAsync();
}