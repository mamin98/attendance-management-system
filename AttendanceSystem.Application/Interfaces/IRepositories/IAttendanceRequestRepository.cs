using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public interface IAttendanceRequestRepository
    : IGenericRepository<AttendanceRequest>
{
    Task<PagedResult<AttendanceRequest>> GetAllWithPaginationAsync(AttendanceRequestSearchDto search);

    Task<IReadOnlyList<AttendanceRequest>>
        GetEmployeeRequestsAsync(Guid employeeId);
}