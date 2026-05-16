using AttendanceSystem.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.API;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AttendanceRequestsController(
    IAttendanceRequestService service) : ControllerBase
{
    readonly IAttendanceRequestService _service = service;

    [HttpGet("paged")]
    public async Task<IActionResult> GetAllWithPagination(
        int page = 1,
        int pageSize = 10)
    {
        PagedResult<AttendanceRequestDto> result =
            await _service.GetAllWithPaginationAsync(page, pageSize);

        if (result is null || result.TotalCount == 0)
        {
            return NotFound(
                ApiResponse<string>.FailureResponse(
                    "No attendance requests found"));
        }

        return Ok(
            ApiResponse<PagedResult<AttendanceRequestDto>>
            .SuccessResponse(result));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<AttendanceRequestDto> result =
            await _service.GetAllAsync();

        if (result is null || result.Count == 0)
        {
            return NotFound(
                ApiResponse<string>.FailureResponse(
                    "No attendance requests found"));
        }

        return Ok(
            ApiResponse<List<AttendanceRequestDto>>
            .SuccessResponse(result));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        AttendanceRequestDto? result =
            await _service.GetByIdAsync(id);

        if (result is null)
        {
            return NotFound(
                ApiResponse<string>.FailureResponse(
                    "Attendance request not found"));
        }

        return Ok(
            ApiResponse<AttendanceRequestDto>
            .SuccessResponse(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
    [FromBody] CreateAttendanceRequestDto dto)
    {
        await _service.CreateAsync(dto);

        return Ok(
            ApiResponse<string>.SuccessResponse(
                null,
                "Attendance request created successfully"));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
    [FromBody] UpdateAttendanceRequestDto dto)
    {
        await _service.UpdateAsync(dto.Id, dto);

        return Ok(
            ApiResponse<string>.SuccessResponse(
                null,
                "Attendance request updated successfully"));
    }

    [Authorize(Roles = "Admin, Manager")]
    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(Guid id)
    {
        await _service.ApproveAsync(id);

        return Ok(
            ApiResponse<string>.SuccessResponse(
                null,
                "Attendance request approved successfully"));
    }

    [Authorize(Roles = "Admin, Manager")]
    [HttpPost("{id}/reject")]
    public async Task<IActionResult> Reject(Guid id)
    {
        await _service.RejectAsync(id);

        return Ok(
            ApiResponse<string>.SuccessResponse(
                null,
                "Attendance request rejected successfully"));
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        await _service.CancelAsync(id);

        return Ok(
            ApiResponse<string>.SuccessResponse(
                null,
                "Attendance request cancelled successfully"));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);

        return Ok(
            ApiResponse<string>.SuccessResponse(
                null,
                "Attendance request deleted successfully"));
    }
}