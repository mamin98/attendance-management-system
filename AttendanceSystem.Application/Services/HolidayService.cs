using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public class HolidayService(IUnitOfWork unitOfWork) : IHolidayService
{
    readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<PagedResult<HolidayDto>> GetAllWithPaginationAsync(HolidaySearchDto search)
    {
        PagedResult<Holiday> data = await _unitOfWork.HolidayRepository.GetAllWithPaginationAsync(search);
        return data.ToPagedDto(x => x.ToDto());
    }

    public async Task<List<HolidayDto>> GetAllAsync()
    {
        IReadOnlyList<Holiday> data = await _unitOfWork.HolidayRepository.GetAllAsync();
        return [.. data.Select(x => x.ToDto())];
    }

    public async Task<HolidayDto?> GetByIdAsync(Guid id)
    {
        Holiday? entity = await _unitOfWork.HolidayRepository.GetByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task CreateAsync(CreateHolidayDto dto)
    {
        if (dto.DepartmentId.HasValue)
        {
            bool departmentExists = await _unitOfWork.DepartmentRepository.IsExistAsync(dto.DepartmentId.Value);
            if (!departmentExists)
                throw new NotFoundException("Department not found");
        }

        Holiday entity = dto.ToEntity();
        await _unitOfWork.HolidayRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, UpdateHolidayDto dto)
    {
        Holiday? entity = await _unitOfWork.HolidayRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Holiday not found");

        if (dto.DepartmentId.HasValue)
        {
            bool departmentExists = await _unitOfWork.DepartmentRepository.IsExistAsync(dto.DepartmentId.Value);
            if (!departmentExists)
                throw new NotFoundException("Department not found");
        }

        dto.UpdateEntity(entity);

        _unitOfWork.HolidayRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        Holiday? entity = await _unitOfWork.HolidayRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Holiday not found");

        _unitOfWork.HolidayRepository.Delete(entity);
        await _unitOfWork.SaveChangesAsync();
    }
}