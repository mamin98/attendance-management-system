using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public class ShiftService(IUnitOfWork unitOfWork) : IShiftService
{
    readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<PagedResult<ShiftDto>> GetAllWithPaginationAsync(ShiftSearchDto search)
    {
        PagedResult<Shift> data = await _unitOfWork.ShiftRepository.GetAllWithPaginationAsync(search);
        return data.ToPagedDto(x => x.ToDto());
    }

    public async Task<List<ShiftDto>> GetAllAsync()
    {
        IReadOnlyList<Shift> data = await _unitOfWork.ShiftRepository.GetAllAsync();
        return [.. data.Select(x => x.ToDto())];
    }

    public async Task<ShiftDto?> GetByIdAsync(Guid id)
    {
        Shift? entity = await _unitOfWork.ShiftRepository.GetByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task CreateAsync(CreateShiftDto dto)
    {
        Shift shift = Shift.Create(dto.NameEnglish, dto.NameArabic, dto.StartDate, dto.EndDate, dto.GracePeriodMinutes);

        await _unitOfWork.ShiftRepository.AddAsync(shift);

        await BuildDaysAndDetailsAsync(shift.Id, dto.Days);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, UpdateShiftDto dto)
    {
        Shift? shift = await _unitOfWork.ShiftRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Shift not found");

        shift.Update(dto.NameEnglish, dto.NameArabic, dto.StartDate, dto.EndDate, dto.GracePeriodMinutes);
        _unitOfWork.ShiftRepository.Update(shift);

        IReadOnlyList<ShiftDay> existingDays = await _unitOfWork.ShiftDayRepository.GetByShiftIdAsync(id);
        if (existingDays.Count > 0)
            _unitOfWork.ShiftDayRepository.RemoveRange(existingDays);

        await BuildDaysAndDetailsAsync(id, dto.Days);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        Shift? entity = await _unitOfWork.ShiftRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Shift not found");

        _unitOfWork.ShiftRepository.Delete(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<List<EmployeeShiftDto>> GetEmployeeShiftHistoryAsync(Guid employeeId)
    {
        bool employeeExists = await _unitOfWork.EmployeeRepository.IsExistAsync(employeeId);
        if (!employeeExists)
            throw new NotFoundException("Employee not found");

        IReadOnlyList<EmployeeShift> data = await _unitOfWork.EmployeeShiftRepository.GetByEmployeeIdAsync(employeeId);

        return [.. data.Select(x => x.ToDto())];
    }

    public async Task AssignShiftAsync(AssignShiftDto dto)
    {
        bool employeeExists = await _unitOfWork.EmployeeRepository.IsExistAsync(dto.EmployeeId);
        if (!employeeExists)
            throw new NotFoundException("Employee not found");

        bool shiftExists = await _unitOfWork.ShiftRepository.IsExistAsync(dto.ShiftId);
        if (!shiftExists)
            throw new NotFoundException("Shift not found");

        EmployeeShift? active = await _unitOfWork.EmployeeShiftRepository.GetActiveByEmployeeIdAsync(dto.EmployeeId);

        if (active is not null)
        {
            active.Terminate(dto.StartDate);
            _unitOfWork.EmployeeShiftRepository.Update(active);
        }

        EmployeeShift assignment = EmployeeShift.Create(dto.EmployeeId, dto.ShiftId, dto.StartDate, dto.EndDate);

        await _unitOfWork.EmployeeShiftRepository.AddAsync(assignment);
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task BuildDaysAndDetailsAsync(Guid shiftId, List<CreateShiftDayDto> days)
    {
        List<ShiftDay> shiftDays = [];
        List<ShiftDayDetail> shiftDayDetails = [];

        foreach (CreateShiftDayDto dayDto in days)
        {
            ShiftDay shiftDay = ShiftDay.Create(shiftId, dayDto.IsOffDay, dayDto.Day);
            shiftDays.Add(shiftDay);

            if (dayDto.IsOffDay) continue;

            foreach (CreateShiftDayDetailDto detailDto in dayDto.ShiftDayDetails)
                shiftDayDetails.Add(ShiftDayDetail.Create(shiftId, shiftDay.Id, detailDto.FromTime, detailDto.ToTime));
        }

        if (shiftDays.Count > 0)
            await _unitOfWork.ShiftDayRepository.AddRangeAsync(shiftDays);

        if (shiftDayDetails.Count > 0)
            await _unitOfWork.ShiftDayDetailRepository.AddRangeAsync(shiftDayDetails);
    }
}