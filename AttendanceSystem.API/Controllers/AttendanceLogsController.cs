using AttendanceSystem.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.API;

[Authorize(Roles = "Admin, Manager")]
[ApiController]
[Route("api/[controller]")]
public class AttendanceLogsController(IAttendanceLogService service) : ControllerBase
{
    readonly IAttendanceLogService _service = service;

    [HttpGet("paged")]
    public async Task<IActionResult> GetAllWithPagination([FromQuery] AttendanceLogSearchDto search)
    {
        PagedResult<AttendanceLogDto> result = await _service.GetAllWithPaginationAsync(search);

        if (result.TotalCount == 0)
            return NotFound(ApiResponse<string>.FailureResponse("No attendance logs found"));

        return Ok(ApiResponse<PagedResult<AttendanceLogDto>>.SuccessResponse(result));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        AttendanceLogDto? result = await _service.GetByIdAsync(id);

        if (result is null)
            return NotFound(ApiResponse<string>.FailureResponse("Attendance log not found"));

        return Ok(ApiResponse<AttendanceLogDto>.SuccessResponse(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAttendanceLogDto dto)
    {
        await _service.CreateAsync(dto);
        return Ok(ApiResponse<string>.SuccessResponse(null, "Attendance log created successfully"));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAttendanceLogDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return Ok(ApiResponse<string>.SuccessResponse(null, "Attendance log updated successfully"));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return Ok(ApiResponse<string>.SuccessResponse(null, "Attendance log deleted successfully"));
    }
}