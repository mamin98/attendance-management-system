using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public class AttendanceRequestService(
    IAttendanceRequestRepository repository,
    IUnitOfWork unitOfWork) : IAttendanceRequestService
{
    private readonly IAttendanceRequestRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Guid> CreateAsync(CreateAttendanceRequestDto create)
    {
        var entity = AttendanceRequest.Create(
            create.EmployeeId,
            create.RequestType,
            create.RequestDate,
            create.FromTime,
            create.ToTime,
            create.Reason);

        await _repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return entity.Id;
    }

    public async Task<List<AttendanceRequestDto>> GetAllAsync()
    {
        var data = await _repository.GetAllAsync();

        return [.. data.Select(x => x.ToDto())];
    }

    public async Task<AttendanceRequestDto?> GetByIdAsync(Guid id)
    {
        AttendanceRequest? entity = await _repository.GetByIdAsync(id);

        return entity?.ToDto();
    }

    public async Task UpdateAsync(Guid id, UpdateAttendanceRequestDto dto)
    {
        AttendanceRequest? entity = await _repository.GetByIdAsync(id);

        if (entity is null)
            throw new Exception("Attendance request not found");

        entity.Update(
            dto.RequestDate,
            dto.FromTime,
            dto.ToTime,
            dto.Reason);

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