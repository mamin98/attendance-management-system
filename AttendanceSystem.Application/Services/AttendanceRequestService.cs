using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public class AttendanceRequestService(
    IAttendanceRequestRepository repository,
    IUnitOfWork unitOfWork) : IAttendanceRequestService
{
    readonly IAttendanceRequestRepository _repository = repository;
    readonly IUnitOfWork _unitOfWork = unitOfWork;

    
    public async Task<PagedResult<AttendanceRequestDto>> GetAllWithPaginationAsync(
    int page,
    int pageSize)
    {
        PagedResult<AttendanceRequest> data = await _repository.GetAllWithPaginationAsync(page, pageSize);

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
        IReadOnlyList<AttendanceRequest> data = await _repository.GetAllAsync();

        return [.. data.Select(x => x.ToDto())];
    }
    
    public async Task<AttendanceRequestDto?> GetByIdAsync(Guid id)
    {
        AttendanceRequest? entity = await _repository.GetByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task CreateAsync(CreateAttendanceRequestDto dto)
    {
        AttendanceRequest entity = dto.ToEntity();

        await _repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(Guid id, UpdateAttendanceRequestDto dto)
    {
        AttendanceRequest? entity = await _repository.GetByIdAsync(id);

        if (entity is null)
            throw new Exception("Attendance request not found");

        if (entity.RequestStatus != RequestStatus.Pending)
            throw new Exception("Only pending requests can be updated");
        
        dto.UpdateEntity(entity);

        _repository.Update(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task CancelAsync(Guid id)
    {
        AttendanceRequest entity = await _repository.GetByIdAsync(id) ?? throw new Exception("Attendance request not found");
        entity.Cancel();

        _repository.Update(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ApproveAsync(Guid id)
    {
        AttendanceRequest entity = await _repository.GetByIdAsync(id) ?? throw new Exception("Attendance request not found");
        entity.Approve();

        _repository.Update(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RejectAsync(Guid id)
    {
        AttendanceRequest entity = await _repository.GetByIdAsync(id) ?? throw new Exception("Attendance request not found");
        entity.Reject();

        _repository.Update(entity);
        await _unitOfWork.SaveChangesAsync();
    }

}