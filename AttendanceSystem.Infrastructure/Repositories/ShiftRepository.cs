using System.Linq.Expressions;
using AttendanceSystem.Application;
using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Infrastructure;

public class ShiftRepository(AttendanceDbContext context)
    : GenericRepository<Shift>(context), IShiftRepository
{
    public async Task<PagedResult<Shift>> GetAllWithPaginationAsync(ShiftSearchDto search)
    {
        Expression<Func<Shift, bool>>? filter = null;

        if (!string.IsNullOrWhiteSpace(search.Search))
        {
            string term = search.Search.ToLower();
            filter = x => 
                x.NameEnglish.ToLower().Contains(term) 
                || x.NameArabic.ToLower().Contains(term);
        }

        return await GetAllWithPaginationAsync(search.Page, search.PageSize, filter: filter);
    }

    public override async Task<Shift?> GetByIdAsync(Guid id)
        => await _context.Shifts
            .Include(x => x.ShiftDays)
                .ThenInclude(d => d.ShiftDayDetails)
            .FirstOrDefaultAsync(x => x.Id == id);
}