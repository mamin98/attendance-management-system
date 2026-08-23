using System.Linq.Expressions;
using AttendanceSystem.Application;
using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Infrastructure;

public class AttendancePolicyRepository(AttendanceDbContext context)
    : GenericRepository<AttendancePolicy>(context), IAttendancePolicyRepository
{
    public async Task<PagedResult<AttendancePolicy>> GetAllWithPaginationAsync(AttendancePolicySearchDto search)
    {
        Expression<Func<AttendancePolicy, bool>>? filter = null;

        if (!string.IsNullOrWhiteSpace(search.Search))
        {
            string term = search.Search.ToLower();
            filter = x => 
                x.NameEnglish.ToLower().Contains(term) 
                || x.NameArabic.ToLower().Contains(term);
        }

        return await GetAllWithPaginationAsync(
            search.Page,
            search.PageSize,
            filter: filter,
            include: q => q.Include(x => x.Departments));
    }

    public override async Task<AttendancePolicy?> GetByIdAsync(Guid id)
        => await _context.AttendancePolicies
            .Include(x => x.Departments)
            .FirstOrDefaultAsync(x => x.Id == id);
}