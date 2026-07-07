using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public interface IAttendancePolicyService
{
    Task<PagedResult<AttendancePolicyDto>> GetAllWithPaginationAsync(AttendancePolicySearchDto search);
    Task<List<AttendancePolicyDto>> GetAllAsync();
    Task<AttendancePolicyDto?> GetByIdAsync(Guid id);
    Task CreateAsync(CreateAttendancePolicyDto dto);
    Task UpdateAsync(Guid id, UpdateAttendancePolicyDto dto);
    Task DeleteAsync(Guid id);
    Task AssignToDepartmentAsync(AssignPolicyToDepartmentDto dto);
    Task ValidateRequestAgainstPolicyAsync(
        Guid employeeId, RequestType requestType, DateTime requestDate, TimeSpan? fromTime, TimeSpan? toTime);
}