using AttendanceSystem.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.API;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ShiftController(IShiftService service) : ControllerBase
{
    readonly IShiftService _service = service;

    [HttpGet("paged")]
    public async Task<IActionResult> GetAllWithPagination([FromQuery] ShiftSearchDto search)
    {
        PagedResult<ShiftDto> result = await _service.GetAllWithPaginationAsync(search);

        if (result.TotalCount == 0)
            return NotFound(ApiResponse<string>.FailureResponse("No shifts found"));

        return Ok(ApiResponse<PagedResult<ShiftDto>>.SuccessResponse(result));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<ShiftDto> result = await _service.GetAllAsync();

        if (result.Count == 0)
            return NotFound(ApiResponse<string>.FailureResponse("No shifts found"));

        return Ok(ApiResponse<List<ShiftDto>>.SuccessResponse(result));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        ShiftDto? result = await _service.GetByIdAsync(id);

        if (result is null)
            return NotFound(ApiResponse<string>.FailureResponse("Shift not found"));

        return Ok(ApiResponse<ShiftDto>.SuccessResponse(result));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateShiftDto dto)
    {
        await _service.CreateAsync(dto);
        return Ok(ApiResponse<string>.SuccessResponse(null, "Shift created successfully"));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateShiftDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return Ok(ApiResponse<string>.SuccessResponse(null, "Shift updated successfully"));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return Ok(ApiResponse<string>.SuccessResponse(null, "Shift deleted successfully"));
    }

    [HttpGet("employee/{employeeId}")]
    public async Task<IActionResult> GetEmployeeShiftHistory(Guid employeeId)
    {
        List<EmployeeShiftDto> result = await _service.GetEmployeeShiftHistoryAsync(employeeId);

        if (result.Count == 0)
            return NotFound(ApiResponse<string>.FailureResponse("No shift assignments found for this employee"));

        return Ok(ApiResponse<List<EmployeeShiftDto>>.SuccessResponse(result));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("assign")]
    public async Task<IActionResult> AssignShift([FromBody] AssignShiftDto dto)
    {
        await _service.AssignShiftAsync(dto);
        return Ok(ApiResponse<string>.SuccessResponse(null, "Shift assigned successfully"));
    }
}