using System.Linq.Expressions;
using AttendanceSystem.Application;
using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Infrastructure;

public class LeaveRequestRepository(AttendanceDbContext context)
    : GenericRepository<LeaveRequest>(context), ILeaveRequestRepository
{
    public async Task<PagedResult<LeaveRequest>> GetAllWithPaginationAsync(LeaveRequestSearchDto search)
    {
        IQueryable<LeaveRequest> q = _context.LeaveRequests
            .Include(x => x.Employee)
            .Include(x => x.LeaveType)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search.Search))
        {
            string term = search.Search.ToLower();
            q = q.Where(x => x.Employee!.NameEnglish.ToLower().Contains(term)
                || x.Employee!.NameArabic.ToLower().Contains(term));
        }

        if (search.EmployeeId.HasValue)
            q = q.Where(x => x.EmployeeId == search.EmployeeId);

        if (search.LeaveTypeId.HasValue)
            q = q.Where(x => x.LeaveTypeId == search.LeaveTypeId);

        if (search.Status.HasValue)
            q = q.Where(x => x.Status == search.Status);

        if (search.FromDate.HasValue)
            q = q.Where(x => x.StartDate >= search.FromDate);

        if (search.ToDate.HasValue)
            q = q.Where(x => x.EndDate <= search.ToDate);

        int totalCount = await q.CountAsync();

        List<LeaveRequest> items = await q
            .OrderByDescending(x => x.StartDate)
            .Skip((search.Page - 1) * search.PageSize)
            .Take(search.PageSize)
            .ToListAsync();

        return new PagedResult<LeaveRequest>
        {
            Items = items,
            TotalCount = totalCount,
            Page = search.Page,
            PageSize = search.PageSize
        };
    }

    public async Task<IReadOnlyList<LeaveRequest>> GetEmployeeRequestsAsync(Guid employeeId)
        => await _context.LeaveRequests
            .AsNoTracking()
            .Include(x => x.LeaveType)
            .Where(x => x.EmployeeId == employeeId)
            .OrderByDescending(x => x.StartDate)
            .ToListAsync();

    public override async Task<LeaveRequest?> GetByIdAsync(Guid id)
        => await _context.LeaveRequests
            .Include(x => x.Employee)
            .Include(x => x.LeaveType)
            .FirstOrDefaultAsync(x => x.Id == id);
}