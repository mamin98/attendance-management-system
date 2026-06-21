using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public class DepartmentService(IUnitOfWork unitOfWork) : IDepartmentService
{
    readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<PagedResult<DepartmentDto>> GetAllWithPaginationAsync(DepartmentSearchDto search)
    {
        PagedResult<Department> data = await _unitOfWork.DepartmentRepository.GetAllWithPaginationAsync(search);

        return new PagedResult<DepartmentDto>
        {
            Items = [.. data.Items.Select(x => x.ToDto())],
            TotalCount = data.TotalCount,
            Page = data.Page,
            PageSize = data.PageSize
        };
    }

    public async Task<List<DepartmentDto>> GetAllAsync()
    {
        IReadOnlyList<Department> data = await _unitOfWork.DepartmentRepository.GetAllAsync();
        return [.. data.Select(x => x.ToDto())];
    }

    public async Task<DepartmentDto?> GetByIdAsync(Guid id)
    {
        Department? entity = await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task CreateAsync(CreateDepartmentDto dto)
    {
        if (dto.ManagerId.HasValue)
        {
            bool managerExists = await _unitOfWork.EmployeeRepository
                .IsExistAsync(dto.ManagerId.Value);

            if (!managerExists)
                throw new NotFoundException("Manager not found");
        }

        Department department = Department.Create(
            dto.ManagerId,
            dto.NameEnglish,
            dto.NameArabic);

        await _unitOfWork.DepartmentRepository.AddAsync(department);
        await _unitOfWork.SaveChangesAsync();

        // Department? created = await _unitOfWork.DepartmentRepository.GetByIdAsync(department.Id);
        // return created!.ToDto();
    }

    public async Task UpdateAsync(Guid id, UpdateDepartmentDto dto)
    {
        Department? entity = await _unitOfWork.DepartmentRepository
            .GetByIdAsync(id)
            ?? throw new NotFoundException("Department not found");

        if (dto.ManagerId.HasValue)
        {
            bool managerExists = await _unitOfWork.EmployeeRepository
                .IsExistAsync(dto.ManagerId.Value);

            if (!managerExists)
                throw new NotFoundException("Manager not found");
        }

        entity.Update(dto.ManagerId, dto.NameEnglish, dto.NameArabic);

        _unitOfWork.DepartmentRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        // Department? updated = await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
        // return updated!.ToDto();
    }

    public async Task DeleteAsync(Guid id)
    {
        Department? entity = await _unitOfWork.DepartmentRepository
            .GetByIdAsync(id)
            ?? throw new NotFoundException("Department not found");

        entity.SoftDelete();
        _unitOfWork.DepartmentRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync();
    }

}
