namespace AttendanceSystem.Application;

public interface IEmployeeService
{
    Task<PagedResult<EmployeeDto>> GetAllWithPaginationAsync(EmployeeSearchDto search);
    Task<List<EmployeeDto>> GetAllAsync();    
    Task<EmployeeDto?> GetByIdAsync(Guid id);
    Task CreateAsync(CreateEmployeeDto dto);
    Task UpdateAsync(Guid id, UpdateEmployeeDto dto);
    Task DeactivateAsync(Guid id);
    //Task ActivateAsync(Guid id);
   Task AssignDepartmentsAsync(Guid employeeId, List<Guid> departmentIds);
}
