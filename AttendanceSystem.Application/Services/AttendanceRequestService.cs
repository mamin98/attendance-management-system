using AttendanceSystem.Domain;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application;

public class AttendanceRequestService(IUnitOfWork unitOfWork,
    IEmailService emailService,
    IAttendancePolicyService policyService,
    ILogger<AttendanceRequestService> logger) : IAttendanceRequestService
{
    readonly IUnitOfWork _unitOfWork = unitOfWork;
    readonly IEmailService _emailService = emailService;
    readonly IAttendancePolicyService _policyService = policyService;
    readonly ILogger<AttendanceRequestService> _logger = logger;

    public async Task<PagedResult<AttendanceRequestDto>> GetAllWithPaginationAsync(
    AttendanceRequestSearchDto searchDto)
    {
        PagedResult<AttendanceRequest> data = await _unitOfWork.AttendanceRequestRepository.GetAllWithPaginationAsync(searchDto);

        return new PagedResult<AttendanceRequestDto>
        {
            Items = [.. data.Items.Select(x => x.ToDto())],
            TotalCount = data.TotalCount,
            Page = data.Page,
            PageSize = data.PageSize
        };
    }

    public async Task<List<AttendanceRequestDto>> GetAllAsync()
    {
        IReadOnlyList<AttendanceRequest> data = await _unitOfWork.AttendanceRequestRepository.GetAllAsync();

        return [.. data.Select(x => x.ToDto())];
    }

    public async Task<AttendanceRequestDto?> GetByIdAsync(Guid id)
    {
        AttendanceRequest? entity = await _unitOfWork.AttendanceRequestRepository.GetByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task<List<AttendanceRequestDto>> GetEmployeeRequestsAsync(Guid employeeId)
    {
        IReadOnlyList<AttendanceRequest> employeeRequests = await _unitOfWork.AttendanceRequestRepository
            .GetEmployeeRequestsAsync(employeeId);

        return [.. employeeRequests.Select(x => x.ToDto())];
    }

    public async Task CreateAsync(CreateAttendanceRequestDto dto)
    {
        bool employeeIsExist = await _unitOfWork.EmployeeRepository.IsExistAsync(dto.EmployeeId);
        if (!employeeIsExist)
            throw new NotFoundException("Employee not found");

         await _policyService.ValidateRequestAgainstPolicyAsync(
            dto.EmployeeId, dto.RequestType, dto.RequestDate, dto.FromTime, dto.ToTime);

        AttendanceRequest entity = dto.ToEntity();

        await _unitOfWork.AttendanceRequestRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, UpdateAttendanceRequestDto dto)
    {
        AttendanceRequest? entity = await _unitOfWork.AttendanceRequestRepository.GetByIdAsync(id);

        if (entity is null)
            throw new NotFoundException("Attendance request not found");

        if (entity.RequestStatus != RequestStatus.Pending)
            throw new ValidationException("Only pending requests can be updated");

        dto.UpdateEntity(entity);

        _unitOfWork.AttendanceRequestRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task CancelAsync(Guid id)
    {
        AttendanceRequest entity = await _unitOfWork.AttendanceRequestRepository.GetByIdAsync(id) ?? throw new NotFoundException("Attendance request not found");
        entity.Cancel();

        _unitOfWork.AttendanceRequestRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ApproveAsync(Guid id)
    {
        AttendanceRequest entity = await _unitOfWork.AttendanceRequestRepository.GetByIdAsync(id) ?? throw new NotFoundException("Attendance request not found");
        entity.Approve();

        _unitOfWork.AttendanceRequestRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        _ = SendStatusEmailAsync(entity, "Approved");

    }

    public async Task RejectAsync(Guid id)
    {
        AttendanceRequest entity = await _unitOfWork.AttendanceRequestRepository.GetByIdAsync(id) ?? throw new NotFoundException("Attendance request not found");
        entity.Reject();

        _unitOfWork.AttendanceRequestRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        _ = SendStatusEmailAsync(entity, "Rejected");
    }

    public async Task DeleteAsync(Guid id)
    {
        AttendanceRequest? entity = await _unitOfWork.AttendanceRequestRepository
            .GetByIdAsync(id);

        if (entity is null)
            throw new NotFoundException("Attendance request not found");

        _unitOfWork.AttendanceRequestRepository.Delete(entity);

        await _unitOfWork.SaveChangesAsync();
    }

    private async Task SendStatusEmailAsync(AttendanceRequest request, string status)
    {
        try
        {
            if (request.Employee?.Email is null) return;

            await _emailService.SendAsync(
                request.Employee.Email,
                $"Attendance Request {status}",
                $"Your {request.RequestType} request for {request.RequestDate:AttendanceSystemConsts.DateFormat} has been {status.ToLower()}."
            );
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex,
                "Failed to send status email for AttendanceRequest {RequestId} to employee {EmployeeId}",
                request.Id, request.EmployeeId);
        }
    }

}