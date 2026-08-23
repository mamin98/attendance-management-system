using AttendanceSystem.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.API;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class PayrollController(IPayrollService service) : ControllerBase
{
    readonly IPayrollService _service = service;

    [HttpPost("generate")]
    public async Task<IActionResult> Generate([FromBody] GenerateReportRequestDto request)
    {
        List<PayrollRecordDto> result = await _service.GeneratePayrollAsync(request);

        if (result.Count == 0)
            return NotFound(ApiResponse<string>.FailureResponse("No payroll could be generated — check that employees have a salary structure configured"));

        return Ok(ApiResponse<List<PayrollRecordDto>>.SuccessResponse(
            result, $"Generated {result.Count} payslip(s)"));
    }

    [HttpGet("employee/{employeeId}/history")]
    public async Task<IActionResult> GetHistory(Guid employeeId)
    {
        List<PayrollRecordDto> result = await _service.GetEmployeeHistoryAsync(employeeId);

        if (result.Count == 0)
            return NotFound(ApiResponse<string>.FailureResponse("No payroll history found for this employee"));

        return Ok(ApiResponse<List<PayrollRecordDto>>.SuccessResponse(result));
    }
}