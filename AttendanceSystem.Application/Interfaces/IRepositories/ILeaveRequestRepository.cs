using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public interface ILeaveRequestRepository : IGenericRepository<LeaveRequest>
{
    Task<PagedResult<LeaveRequest>> GetAllWithPaginationAsync(LeaveRequestSearchDto search);
    Task<IReadOnlyList<LeaveRequest>> GetEmployeeRequestsAsync(Guid employeeId);
}