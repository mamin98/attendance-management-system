using AttendanceSystem.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.API;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LeaveTypesController(ILeaveTypeService service) : ControllerBase
{
    readonly ILeaveTypeService _service = service;

    [HttpGet("paged")]
    public async Task<IActionResult> GetAllWithPagination([FromQuery] LeaveTypeSearchDto search)
    {
        PagedResult<LeaveTypeDto> result = await _service.GetAllWithPaginationAsync(search);

        if (result.TotalCount == 0)
            return NotFound(ApiResponse<string>.FailureResponse("No leave types found"));

        return Ok(ApiResponse<PagedResult<LeaveTypeDto>>.SuccessResponse(result));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<LeaveTypeDto> result = await _service.GetAllAsync();

        if (result.Count == 0)
            return NotFound(ApiResponse<string>.FailureResponse("No leave types found"));

        return Ok(ApiResponse<List<LeaveTypeDto>>.SuccessResponse(result));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        LeaveTypeDto? result = await _service.GetByIdAsync(id);

        if (result is null)
            return NotFound(ApiResponse<string>.FailureResponse("Leave type not found"));

        return Ok(ApiResponse<LeaveTypeDto>.SuccessResponse(result));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLeaveTypeDto dto)
    {
        await _service.CreateAsync(dto);
        return Ok(ApiResponse<string>.SuccessResponse(null, "Leave type created successfully"));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLeaveTypeDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return Ok(ApiResponse<string>.SuccessResponse(null, "Leave type updated successfully"));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return Ok(ApiResponse<string>.SuccessResponse(null, "Leave type deleted successfully"));
    }

    [HttpGet("employee/{employeeId}/balances")]
    public async Task<IActionResult> GetEmployeeBalances(Guid employeeId, [FromQuery] int? year)
    {
        List<EmployeeLeaveBalanceDto> result = await _service.GetEmployeeBalancesAsync(employeeId, year);

        if (result.Count == 0)
            return NotFound(ApiResponse<string>.FailureResponse("No leave balances found for this employee"));

        return Ok(ApiResponse<List<EmployeeLeaveBalanceDto>>.SuccessResponse(result));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("allocate-balance")]
    public async Task<IActionResult> AllocateBalance([FromBody] AllocateLeaveBalanceDto dto)
    {
        await _service.AllocateBalanceAsync(dto);
        return Ok(ApiResponse<string>.SuccessResponse(null, "Leave balance allocated successfully"));
    }
}