using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public class AttendanceRequestService(IUnitOfWork unitOfWork) : IAttendanceRequestService
{
    readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    public async Task<PagedResult<AttendanceRequestDto>> GetAllWithPaginationAsync(
    int page,
    int pageSize)
    {
        PagedResult<AttendanceRequest> data = await _unitOfWork.AttendanceRequestRepository.GetAllWithPaginationAsync(page, pageSize);

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

    public async Task CreateAsync(CreateAttendanceRequestDto dto)
    {
        bool employeeIsExist = await _unitOfWork.EmployeeRepository.IsExistAsync(dto.EmployeeId);
        if (!employeeIsExist)
            throw new NotFoundException("Employee not found"); 

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
    }

    public async Task RejectAsync(Guid id)
    {
        AttendanceRequest entity = await _unitOfWork.AttendanceRequestRepository.GetByIdAsync(id) ?? throw new NotFoundException("Attendance request not found");
        entity.Reject();

        _unitOfWork.AttendanceRequestRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync();
    }

}