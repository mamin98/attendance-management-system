using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public class EmployeeDepartmentService(IUnitOfWork unitOfWork)
    : IEmployeeDepartmentService
{
    readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<List<EmployeeDepartmentDto>> GetByEmployeeIdAsync(Guid employeeId)
    {
        bool employeeExists = await _unitOfWork.EmployeeRepository
            .IsExistAsync(employeeId);

        if (!employeeExists)
            throw new NotFoundException("Employee not found");

        IReadOnlyList<EmployeeDepartment> data = await _unitOfWork
            .EmployeeDepartmentRepository
            .GetByEmployeeIdAsync(employeeId);

        return data
            .Select(x => new EmployeeDepartmentDto
            {
                DepartmentId = x.DepartmentId ?? Guid.Empty,
                NameEnglish  = x.Department?.NameEnglish ?? string.Empty,
                NameArabic   = x.Department?.NameArabic  ?? string.Empty,
                StartDate    = x.StartDate,
                EndDate      = x.EndDate
               // IsActive     = x.IsActive
            })
            .ToList();
    }

    public async Task AssignDepartmentsAsync(Guid employeeId, AssignDepartmentsDto dto)
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
            .Where(x => 
            //x.IsActive &&
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

    public async Task TerminateAsync(Guid employeeId, Guid departmentId, string? endDate = null)
    {
        IReadOnlyList<EmployeeDepartment> existing = await _unitOfWork
            .EmployeeDepartmentRepository
            .GetByEmployeeIdAsync(employeeId);

        EmployeeDepartment? assignment = existing
            .FirstOrDefault(x => x.DepartmentId == departmentId 
            //&& x.IsActive
            )
            ?? throw new NotFoundException("Active department assignment not found");

        assignment.Terminate(endDate);

        _unitOfWork.EmployeeDepartmentRepository.Update(assignment);
        await _unitOfWork.SaveChangesAsync();
    }
}