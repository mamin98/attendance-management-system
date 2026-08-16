using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public class AttendanceLogService(IUnitOfWork unitOfWork) : IAttendanceLogService
{
    readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<PagedResult<AttendanceLogDto>> GetAllWithPaginationAsync(AttendanceLogSearchDto search)
    {
        PagedResult<AttendanceLog> data = await _unitOfWork.AttendanceLogRepository.GetAllWithPaginationAsync(search);
        return data.ToPagedDto(x => x.ToDto());
    }

    public async Task<AttendanceLogDto?> GetByIdAsync(Guid id)
    {
        AttendanceLog? entity = await _unitOfWork.AttendanceLogRepository.GetByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task CreateAsync(CreateAttendanceLogDto dto)
    {
        bool employeeExists = await _unitOfWork.EmployeeRepository.IsExistAsync(dto.EmployeeId);
        if (!employeeExists)
            throw new NotFoundException("Employee not found");

        AttendanceLog? existing = await _unitOfWork.AttendanceLogRepository
            .GetByEmployeeAndDateAsync(dto.EmployeeId, DateAndTimeHelperConvert.GetDateTime(dto.Date));

        if (existing is not null)
            throw new ValidationException("An attendance log already exists for this employee on this date");

        AttendanceLog entity = AttendanceLog.Create(dto.EmployeeId, dto.Date, dto.CheckIn, dto.CheckOut, "Manual");

        await _unitOfWork.AttendanceLogRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, UpdateAttendanceLogDto dto)
    {
        AttendanceLog? entity = await _unitOfWork.AttendanceLogRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Attendance log not found");

        entity.Update(dto.CheckIn, dto.CheckOut, "Manual");

        _unitOfWork.AttendanceLogRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        AttendanceLog? entity = await _unitOfWork.AttendanceLogRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Attendance log not found");

        _unitOfWork.AttendanceLogRepository.Delete(entity);
        await _unitOfWork.SaveChangesAsync();
    }
}