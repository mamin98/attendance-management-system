using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public interface IAttendanceRequestRepository
    : IGenericRepository<AttendanceRequest>
{
    Task<IReadOnlyList<AttendanceRequest>>
        GetEmployeeRequestsAsync(Guid employeeId);
}