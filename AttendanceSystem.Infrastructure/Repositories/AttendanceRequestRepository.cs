using Microsoft.EntityFrameworkCore;
using AttendanceSystem.Application;
using AttendanceSystem.Domain;

namespace AttendanceSystem.Infrastructure;

public class AttendanceRequestRepository(AttendanceDbContext context)
        : GenericRepository<AttendanceRequest>(context), IAttendanceRequestRepository
{
    public async Task<PagedResult<AttendanceRequest>> GetAllWithPaginationAsync(AttendanceRequestSearchDto query)
    {       
        IQueryable<AttendanceRequest> q = _context.AttendanceRequests
            .Include(x => x.Employee)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            string search = query.Search.ToLower();

            q = q.Where(x =>
                 x.Employee!.NameEnglish.ToLower().Contains(search)
                || x.Employee!.NameArabic.ToLower().Contains(search));
        }

        if (query.EmployeeId.HasValue)
            q = q.Where(x => x.EmployeeId == query.EmployeeId);

        if (query.RequestType.HasValue)
            q = q.Where(x => x.RequestType == query.RequestType);

        if (query.RequestStatus.HasValue)
            q = q.Where(x => x.RequestStatus == query.RequestStatus);

        if (query.FromDate.HasValue)
            q = q.Where(x => x.RequestDate >= query.FromDate);

        if (query.ToDate.HasValue)
            q = q.Where(x => x.RequestDate <= query.ToDate);

        int totalCount = await q.CountAsync();

        var items = await q
            .OrderByDescending(x => x.RequestDate)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return new PagedResult<AttendanceRequest>
        {
            Items = items,
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<IReadOnlyList<AttendanceRequest>> GetEmployeeRequestsAsync(Guid employeeId)
    {
        return await _context.AttendanceRequests
            .AsNoTracking()
            .Include(x => x.Employee)
            .Where(x => x.EmployeeId == employeeId)
            .ToListAsync();
    }
}