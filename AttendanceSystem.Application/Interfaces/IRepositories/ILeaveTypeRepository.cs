using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public interface ILeaveTypeRepository : IGenericRepository<LeaveType>
{
    Task<PagedResult<LeaveType>> GetAllWithPaginationAsync(LeaveTypeSearchDto search);
}