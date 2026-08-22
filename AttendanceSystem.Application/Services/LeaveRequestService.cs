using AttendanceSystem.Domain;
using Hangfire;

namespace AttendanceSystem.Application;

public class LeaveRequestService(
    IUnitOfWork unitOfWork,
    IBackgroundJobClient backgroundJobClient) : ILeaveRequestService
{
    readonly IUnitOfWork _unitOfWork = unitOfWork;
    readonly IBackgroundJobClient _backgroundJobClient = backgroundJobClient;

    public async Task<PagedResult<LeaveRequestDto>> GetAllWithPaginationAsync(LeaveRequestSearchDto search)
    {
        PagedResult<LeaveRequest> data = await _unitOfWork.LeaveRequestRepository.GetAllWithPaginationAsync(search);
        return data.ToPagedDto(x => x.ToDto());
    }

    public async Task<List<LeaveRequestDto>> GetAllAsync()
    {
        IReadOnlyList<LeaveRequest> data = await _unitOfWork.LeaveRequestRepository.GetAllAsync();
        return [.. data.Select(x => x.ToDto())];
    }

    public async Task<LeaveRequestDto?> GetByIdAsync(Guid id)
    {
        LeaveRequest? entity = await _unitOfWork.LeaveRequestRepository.GetByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task<List<LeaveRequestDto>> GetEmployeeRequestsAsync(Guid employeeId)
    {
        IReadOnlyList<LeaveRequest> data = await _unitOfWork.LeaveRequestRepository.GetEmployeeRequestsAsync(employeeId);
        return [.. data.Select(x => x.ToDto())];
    }

    public async Task CreateAsync(CreateLeaveRequestDto dto)
    {
        bool employeeExists = await _unitOfWork.EmployeeRepository.IsExistAsync(dto.EmployeeId);
        if (!employeeExists)
            throw new NotFoundException("Employee not found");

        LeaveType? leaveType = await _unitOfWork.LeaveTypeRepository.GetByIdAsync(dto.LeaveTypeId)
            ?? throw new NotFoundException("Leave type not found");

        LeaveRequest entity = LeaveRequest.Create(dto.EmployeeId, dto.LeaveTypeId, dto.StartDate, dto.EndDate, dto.Reason);

        await EnsureSufficientBalanceAsync(dto.EmployeeId, dto.LeaveTypeId, dto.StartDate.Year, entity.DaysCount);

        await _unitOfWork.LeaveRequestRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, UpdateLeaveRequestDto dto)
    {
        LeaveRequest? entity = await _unitOfWork.LeaveRequestRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Leave request not found");

        if (entity.Status != LeaveRequestStatus.Pending)
            throw new ValidationException("Only pending leave requests can be updated");

        entity.Update(dto.StartDate, dto.EndDate, dto.Reason);

        await EnsureSufficientBalanceAsync(entity.EmployeeId, entity.LeaveTypeId, dto.StartDate.Year, entity.DaysCount);

        _unitOfWork.LeaveRequestRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ApproveAsync(Guid id)
    {
        LeaveRequest entity = await _unitOfWork.LeaveRequestRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Leave request not found");

        entity.Approve();

        EmployeeLeaveBalance balance = await _unitOfWork.EmployeeLeaveBalanceRepository
            .GetAsync(entity.EmployeeId, entity.LeaveTypeId, entity.StartDate.Year)
            ?? throw new ValidationException("No leave balance found for this employee/leave type/year");

        balance.Consume(entity.DaysCount);

        _unitOfWork.EmployeeLeaveBalanceRepository.Update(balance);
        _unitOfWork.LeaveRequestRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        _backgroundJobClient.Enqueue<ILeaveEmailJob>(x => x.SendLeaveStatusEmailAsync(id, "Approved"));
    }

    public async Task RejectAsync(Guid id)
    {
        LeaveRequest entity = await _unitOfWork.LeaveRequestRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Leave request not found");

        entity.Reject();

        _unitOfWork.LeaveRequestRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        _backgroundJobClient.Enqueue<ILeaveEmailJob>(x => x.SendLeaveStatusEmailAsync(id, "Rejected"));
    }

    public async Task CancelAsync(Guid id)
    {
        LeaveRequest entity = await _unitOfWork.LeaveRequestRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Leave request not found");

        bool wasApproved = entity.Status == LeaveRequestStatus.Approved;

        entity.Cancel();

        if (wasApproved)
        {
            EmployeeLeaveBalance? balance = await _unitOfWork.EmployeeLeaveBalanceRepository
                .GetAsync(entity.EmployeeId, entity.LeaveTypeId, entity.StartDate.Year);

            if (balance is not null)
            {
                balance.Restore(entity.DaysCount);
                _unitOfWork.EmployeeLeaveBalanceRepository.Update(balance);
            }
        }

        _unitOfWork.LeaveRequestRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        LeaveRequest? entity = await _unitOfWork.LeaveRequestRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Leave request not found");

        _unitOfWork.LeaveRequestRepository.Delete(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task EnsureSufficientBalanceAsync(Guid employeeId, Guid leaveTypeId, int year, decimal requiredDays)
    {
        EmployeeLeaveBalance? balance = await _unitOfWork.EmployeeLeaveBalanceRepository
            .GetAsync(employeeId, leaveTypeId, year);

        if (balance is null)
            throw new ValidationException("No leave balance allocated for this employee/leave type/year");

        if (requiredDays > balance.RemainingDays)
            throw new ValidationException($"Insufficient leave balance. Remaining: {balance.RemainingDays} day(s), requested: {requiredDays} day(s)");
    }
}