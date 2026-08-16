using AttendanceSystem.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.API;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class SalaryStructuresController(ISalaryStructureService service) : ControllerBase
{
    readonly ISalaryStructureService _service = service;

    [HttpGet("employee/{employeeId}")]
    public async Task<IActionResult> GetByEmployeeId(Guid employeeId)
    {
        SalaryStructureDto? result = await _service.GetByEmployeeIdAsync(employeeId);

        if (result is null)
            return NotFound(ApiResponse<string>.FailureResponse("No salary structure configured for this employee"));

        return Ok(ApiResponse<SalaryStructureDto>.SuccessResponse(result));
    }

    [HttpPost]
    public async Task<IActionResult> Set([FromBody] CreateSalaryStructureDto dto)
    {
        await _service.SetAsync(dto);
        return Ok(ApiResponse<string>.SuccessResponse(null, "Salary structure saved successfully"));
    }
}