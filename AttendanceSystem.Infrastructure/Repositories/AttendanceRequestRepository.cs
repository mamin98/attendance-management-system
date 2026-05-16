using Microsoft.EntityFrameworkCore;
using AttendanceSystem.Application;
using AttendanceSystem.Domain;

namespace AttendanceSystem.Infrastructure;

public class AttendanceRequestRepository(AttendanceDbContext context)
        : GenericRepository<AttendanceRequest>(context), IAttendanceRequestRepository
{
    public async Task<IReadOnlyList<AttendanceRequest>> GetEmployeeRequestsAsync(Guid employeeId)
    {
        return await _context.AttendanceRequests
            .AsNoTracking()
            .Include(x => x.Employee)
            .Where(x => x.EmployeeId == employeeId)
            .ToListAsync();
    }
}