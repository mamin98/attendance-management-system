using AttendanceSystem.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.API;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EmployeesController(IEmployeeService employeeService) : ControllerBase
{
    private readonly IEmployeeService _employeeService = employeeService;

    [HttpGet]
    public async Task<ActionResult<PagedResult<EmployeeDto>>> GetAll(
        [FromQuery] EmployeeSearchDto search)
    {
        var result = await _employeeService
            .GetAllWithPaginationAsync(search);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDto>> GetById(Guid id)
    {
        var employee = await _employeeService.GetByIdAsync(id);

        if (employee is null)
            return NotFound();

        return Ok(employee);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateEmployeeDto dto)
    {
        await _employeeService.CreateAsync(dto);

        return Created();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateEmployeeDto dto)
    {
        await _employeeService.UpdateAsync(id, dto);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        await _employeeService.DeactivateAsync(id);

        return NoContent();
    }

    [HttpPost("{employeeId:guid}/departments")]
    public async Task<IActionResult> AssignDepartments(
        Guid employeeId,
        [FromBody] AssignDepartmentsDto dto)
    {
        await _employeeService.AssignDepartmentsAsync(
            employeeId,
            dto);

        return NoContent();
    }
}