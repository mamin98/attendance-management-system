using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public class AttendancePolicyService(IUnitOfWork unitOfWork) : IAttendancePolicyService
{
    readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<PagedResult<AttendancePolicyDto>> GetAllWithPaginationAsync(AttendancePolicySearchDto search)
    {
        PagedResult<AttendancePolicy> data = await _unitOfWork.AttendancePolicyRepository.GetAllWithPaginationAsync(search);

        return new PagedResult<AttendancePolicyDto>
        {
            Items = [.. data.Items.Select(x => x.ToDto())],
            TotalCount = data.TotalCount,
            Page = data.Page,
            PageSize = data.PageSize
        };
    }

    public async Task<List<AttendancePolicyDto>> GetAllAsync()
    {
        IReadOnlyList<AttendancePolicy> data = await _unitOfWork.AttendancePolicyRepository.GetAllAsync();
        return [.. data.Select(x => x.ToDto())];
    }

    public async Task<AttendancePolicyDto?> GetByIdAsync(Guid id)
    {
        AttendancePolicy? entity = await _unitOfWork.AttendancePolicyRepository.GetByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task CreateAsync(CreateAttendancePolicyDto dto)
    {
        AttendancePolicy entity = dto.ToEntity();
        await _unitOfWork.AttendancePolicyRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, UpdateAttendancePolicyDto dto)
    {
        AttendancePolicy? entity = await _unitOfWork.AttendancePolicyRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Policy not found");

        dto.UpdateEntity(entity);

        _unitOfWork.AttendancePolicyRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        AttendancePolicy? entity = await _unitOfWork.AttendancePolicyRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Policy not found");

        _unitOfWork.AttendancePolicyRepository.Delete(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task AssignToDepartmentAsync(AssignPolicyToDepartmentDto dto)
    {
        Department? department = await _unitOfWork.DepartmentRepository.GetByIdAsync(dto.DepartmentId)
            ?? throw new NotFoundException("Department not found");

        bool policyExists = await _unitOfWork.AttendancePolicyRepository.IsExistAsync(dto.PolicyId);
        if (!policyExists)
            throw new NotFoundException("Policy not found");

        department.SetPolicyId(dto.PolicyId);

        _unitOfWork.DepartmentRepository.Update(department);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ValidateRequestAgainstPolicyAsync(
        Guid employeeId, RequestType requestType, DateTime requestDate, TimeSpan? fromTime, TimeSpan? toTime)
    {
        Employee? employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(employeeId);
        if (employee is null) return;

        Guid? departmentId = employee.EmployeeDepartments
            .FirstOrDefault(ed => ed.EndDate is null)?.DepartmentId;

        if (departmentId is null) return;

        Department? department = await _unitOfWork.DepartmentRepository.GetByIdAsync(departmentId.Value);
        if (department?.PolicyId is null) return;

        AttendancePolicy? policy = await _unitOfWork.AttendancePolicyRepository.GetByIdAsync(department.PolicyId.Value);
        if (policy is null) return;

        DateTime monthStart = new(requestDate.Year, requestDate.Month, 1);
        DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1);

        List<AttendanceRequest> monthlyRequests = [.. (await _unitOfWork.AttendanceRequestRepository
            .GetEmployeeRequestsAsync(employeeId))
            .Where(x => x.RequestDate >= monthStart
                && x.RequestDate <= monthEnd
                && x.RequestStatus != RequestStatus.Rejected
                && x.RequestStatus != RequestStatus.Cancelled)];

        switch (requestType)
        {
            case RequestType.Permission:
                int permissionCount = monthlyRequests.Count(x => x.RequestType == RequestType.Permission);
                if (permissionCount >= policy.MaxPermissionsPerMonth)
                    throw new ValidationException($"Monthly permission limit ({policy.MaxPermissionsPerMonth}) exceeded for this employee");
                break;

            case RequestType.Remote:
                int remoteCount = monthlyRequests.Count(x => x.RequestType == RequestType.Remote);
                if (remoteCount >= policy.MaxRemoteDaysPerMonth)
                    throw new ValidationException($"Monthly remote work limit ({policy.MaxRemoteDaysPerMonth}) exceeded for this employee");
                break;

            case RequestType.EarlyLeave:
                int earlyLeaveCount = monthlyRequests.Count(x => x.RequestType == RequestType.EarlyLeave);
                if (earlyLeaveCount >= policy.MaxEarlyLeavesPerMonth)
                    throw new ValidationException($"Monthly early leave limit ({policy.MaxEarlyLeavesPerMonth}) exceeded for this employee");
                break;

            case RequestType.Late:
                int lateMinutesThisMonth = monthlyRequests
                    .Where(x => x.RequestType == RequestType.Late && x.FromTime.HasValue && x.ToTime.HasValue)
                    .Sum(x => (int)(x.ToTime!.Value - x.FromTime!.Value).TotalMinutes);

                int incomingMinutes = fromTime.HasValue && toTime.HasValue
                    ? (int)(toTime.Value - fromTime.Value).TotalMinutes
                    : 0;

                if (lateMinutesThisMonth + incomingMinutes > policy.MaxLateMinutesPerMonth)
                    throw new ValidationException($"Monthly late-minutes limit ({policy.MaxLateMinutesPerMonth} min) would be exceeded for this employee");
                break;
        }
    }
}