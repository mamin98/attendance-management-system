using Microsoft.EntityFrameworkCore;
using AttendanceSystem.Application;
using AttendanceSystem.Domain;

namespace AttendanceSystem.Infrastructure;

public class AttendanceRequestRepository
    : GenericRepository<AttendanceRequest>,
      IAttendanceRequestRepository
{
    public AttendanceRequestRepository(
        AttendanceDbContext context)
        : base(context)
    {
    }

    public async Task<IReadOnlyList<AttendanceRequest>>
        GetEmployeeRequestsAsync(Guid employeeId)
    {
        return await _context.AttendanceRequests
            .Where(x => x.EmployeeId == employeeId)
            .ToListAsync();
    }
}