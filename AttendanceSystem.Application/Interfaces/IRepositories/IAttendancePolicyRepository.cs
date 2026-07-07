using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public interface IAttendancePolicyRepository : IGenericRepository<AttendancePolicy>
{
    Task<PagedResult<AttendancePolicy>> GetAllWithPaginationAsync(AttendancePolicySearchDto search);
}