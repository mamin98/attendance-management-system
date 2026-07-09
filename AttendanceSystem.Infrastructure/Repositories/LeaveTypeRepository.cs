using System.Linq.Expressions;
using AttendanceSystem.Application;
using AttendanceSystem.Domain;

namespace AttendanceSystem.Infrastructure;

public class LeaveTypeRepository(AttendanceDbContext context)
    : GenericRepository<LeaveType>(context), ILeaveTypeRepository
{
    public async Task<PagedResult<LeaveType>> GetAllWithPaginationAsync(LeaveTypeSearchDto search)
    {
        Expression<Func<LeaveType, bool>>? filter = null;

        if (!string.IsNullOrWhiteSpace(search.Search))
        {
            string term = search.Search.ToLower();
            filter = x => x.NameEnglish.ToLower().Contains(term) 
            || x.NameArabic.ToLower().Contains(term);
        }

        return await GetAllWithPaginationAsync(search.Page, search.PageSize, filter: filter);
    }
}