using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public class EmployeeService(
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher) : IEmployeeService
{
    readonly IUnitOfWork _unitOfWork = unitOfWork;
    readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<PagedResult<EmployeeDto>> GetAllWithPaginationAsync(EmployeeSearchDto search)
    {
        PagedResult<Employee> data = await _unitOfWork.EmployeeRepository.GetAllWithPaginationAsync(search);

        return new PagedResult<EmployeeDto>
        {
            Items = [.. data.Items.Select(x => x.ToDto())],
            TotalCount = data.TotalCount,
            Page = data.Page,
            PageSize = data.PageSize
        };
    }

    public async Task<List<EmployeeDto>> GetAllAsync()
    {
        IReadOnlyList<Employee> data = await _unitOfWork.EmployeeRepository.GetAllAsync();
        return [.. data.Select(x => x.ToDto())];
    }

    public async Task<EmployeeDto?> GetByIdAsync(Guid id)
    {
        Employee? entity = await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task CreateAsync(CreateEmployeeDto dto)
    {
        bool emailExists = await _unitOfWork.EmployeeRepository
            .IsEmailUniqueAsync(dto.Email);

        if (!emailExists)
            throw new ValidationException("Email is already in use");

        string passwordHash = _passwordHasher.Hash(dto.Password);
        dto.Password = passwordHash;

        Employee entity = dto.ToEntity();

        await _unitOfWork.EmployeeRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        if (dto.DepartmentIds.Count > 0)
            await AssignDepartmentsAsync(entity.Id, dto.DepartmentIds);

        await _unitOfWork.SaveChangesAsync();

        // Employee? created = await _unitOfWork.EmployeeRepository.GetByIdAsync(entity.Id);
        // return created!.ToDto();
    }

    public async Task UpdateAsync(Guid id, UpdateEmployeeDto dto)
    {
        Employee? entity = await _unitOfWork.EmployeeRepository
            .GetByIdAsync(id)
            ?? throw new NotFoundException("Employee not found");

        bool emailUnique = await _unitOfWork.EmployeeRepository
            .IsEmailUniqueAsync(dto.Email, excludeId: id);

        if (!emailUnique)
            throw new ValidationException("Email is already in use");

        entity.Update(dto.Role, dto.NameEnglish, dto.NameArabic, dto.Email);

        _unitOfWork.EmployeeRepository.Update(entity);

        if (dto.DepartmentIds.Count > 0)
            await AssignDepartmentsAsync(id, dto.DepartmentIds);

        await _unitOfWork.SaveChangesAsync();

        // Employee? updated = await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
        // return updated!.ToDto();
    }

    public async Task DeactivateAsync(Guid id)
    {
        Employee? entity = await _unitOfWork.EmployeeRepository
            .GetByIdAsync(id)
            ?? throw new NotFoundException("Employee not found");

        if (entity.IsDeleted)
            throw new ValidationException("Employee is already deactivated");

        entity.SoftDelete();
        _unitOfWork.EmployeeRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    // public async Task ActivateAsync(Guid id)
    // {
    //     Employee? entity = await _unitOfWork.EmployeeRepository
    //         .GetByIdAsync(id)
    //         ?? throw new NotFoundException("Employee not found");

    //     entity.Activate();
    //     _unitOfWork.EmployeeRepository.Update(entity);
    //     await _unitOfWork.SaveChangesAsync();
    // }

    public async Task AssignDepartmentsAsync(
     Guid employeeId,
     AssignDepartmentsDto dto)
    {
        bool employeeExists = await _unitOfWork.EmployeeRepository
            .IsExistAsync(employeeId);

        if (!employeeExists)
            throw new NotFoundException("Employee not found");

        List<Guid> incomingIds = dto.Departments
            .Select(x => x.DepartmentId)
            .ToList();

        IReadOnlyList<EmployeeDepartment> existing = await _unitOfWork
            .EmployeeDepartmentRepository
            .GetByEmployeeIdAsync(employeeId);

        List<Guid> validDeptIds = await _unitOfWork.DepartmentRepository
            .GetExistingIdsAsync(incomingIds);

        List<Guid> invalidIds = incomingIds
            .Except(validDeptIds)
            .ToList();

        if (invalidIds.Count > 0)
            throw new NotFoundException(
                $"Departments not found: {string.Join(", ", invalidIds)}");

        string today = DateTime.UtcNow.ToString(AttendanceSystemConsts.DateFormat);

        List<EmployeeDepartment> toTerminate = existing
            .Where(x => x.IsActive &&
                        !incomingIds.Contains(x.DepartmentId!.Value))
            .ToList();

        foreach (EmployeeDepartment ed in toTerminate)
            ed.Terminate(today);

        if (toTerminate.Count > 0)
            _unitOfWork.EmployeeDepartmentRepository.UpdateRange(toTerminate);

        HashSet<Guid> existingDeptIds = existing
            .Select(x => x.DepartmentId!.Value)
            .ToHashSet();

        List<EmployeeDepartment> toAdd = dto.Departments
            .Where(x => !existingDeptIds.Contains(x.DepartmentId))
            .Select(x => EmployeeDepartment.Create(
                employeeId,
                x.DepartmentId,
                x.StartDate,
                x.EndDate))
            .ToList();

        if (toAdd.Count > 0)
            await _unitOfWork.EmployeeDepartmentRepository.AddRangeAsync(toAdd);

        await _unitOfWork.SaveChangesAsync();
    }
}