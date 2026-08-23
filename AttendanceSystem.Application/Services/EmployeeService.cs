using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public class EmployeeService(
    IEmployeeDepartmentService employeeDepartmentService,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork) : IEmployeeService
{
    readonly IEmployeeDepartmentService _employeeDepartmentService = employeeDepartmentService;
    readonly IPasswordHasher _passwordHasher = passwordHasher;
    readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<PagedResult<EmployeeDto>> GetAllWithPaginationAsync(EmployeeSearchDto search)
    {
        PagedResult<Employee> data = await _unitOfWork.EmployeeRepository.GetAllWithPaginationAsync(search);
        return data.ToPagedDto(x => x.ToDto());
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
        bool isEmailUnique = await _unitOfWork.EmployeeRepository
            .IsEmailUniqueAsync(dto.Email);

        if (!isEmailUnique)
            throw new ValidationException("Email is already in use");

        string passwordHash = _passwordHasher.Hash(dto.Password);
        dto.Password = passwordHash;

        Employee entity = dto.ToEntity();

        await _unitOfWork.EmployeeRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        // if (dto.DepartmentIds.Count > 0)
        //     await AssignDepartmentsAsync(entity.Id, dto.DepartmentIds);

        // await _unitOfWork.SaveChangesAsync();

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
        await _unitOfWork.SaveChangesAsync();

        // if (dto.DepartmentIds.Count > 0)
        //     await AssignDepartmentsAsync(id, dto.DepartmentIds);

        // await _unitOfWork.SaveChangesAsync();

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

    public Task AssignDepartmentsAsync(Guid employeeId, AssignDepartmentsDto dto)
        => _employeeDepartmentService.AssignDepartmentsAsync(employeeId, dto);

    
}