using AttendanceSystem.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.API;

[Authorize]
[ApiController]
[Route("api/employees/{employeeId}/departments")]
public class EmployeeDepartmentsController(IEmployeeDepartmentService service)
    : ControllerBase
{
    readonly IEmployeeDepartmentService _service = service;

    [HttpGet]
    public async Task<IActionResult> GetByEmployee(Guid employeeId)
    {
        List<EmployeeDepartmentDto> result = await _service.GetByEmployeeIdAsync(employeeId);
        return Ok(ApiResponse<List<EmployeeDepartmentDto>>.SuccessResponse(result));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<IActionResult> Assign(Guid employeeId, [FromBody] AssignDepartmentsDto dto)
    {
        await _service.AssignDepartmentsAsync(employeeId, dto);
        return Ok(ApiResponse<string>.SuccessResponse(null, "Departments assigned successfully"));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{departmentId}/terminate")]
    public async Task<IActionResult> Terminate(Guid employeeId, Guid departmentId, [FromQuery] string? endDate)
    {
        await _service.TerminateAsync(employeeId, departmentId, endDate);
        return Ok(ApiResponse<string>.SuccessResponse(null, "Department assignment terminated"));
    }
}