using AttendanceSystem.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.API;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LeaveRequestsController(ILeaveRequestService service) : ControllerBase
{
    readonly ILeaveRequestService _service = service;

    [HttpGet("paged")]
    public async Task<IActionResult> GetAllWithPagination([FromQuery] LeaveRequestSearchDto search)
    {
        PagedResult<LeaveRequestDto> result = await _service.GetAllWithPaginationAsync(search);

        if (result.TotalCount == 0)
            return NotFound(ApiResponse<string>.FailureResponse("No leave requests found"));

        return Ok(ApiResponse<PagedResult<LeaveRequestDto>>.SuccessResponse(result));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<LeaveRequestDto> result = await _service.GetAllAsync();

        if (result.Count == 0)
            return NotFound(ApiResponse<string>.FailureResponse("No leave requests found"));

        return Ok(ApiResponse<List<LeaveRequestDto>>.SuccessResponse(result));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        LeaveRequestDto? result = await _service.GetByIdAsync(id);

        if (result is null)
            return NotFound(ApiResponse<string>.FailureResponse("Leave request not found"));

        return Ok(ApiResponse<LeaveRequestDto>.SuccessResponse(result));
    }

    [HttpGet("employee/{employeeId}")]
    public async Task<IActionResult> GetEmployeeRequests(Guid employeeId)
    {
        List<LeaveRequestDto> result = await _service.GetEmployeeRequestsAsync(employeeId);

        if (result.Count == 0)
            return NotFound(ApiResponse<string>.FailureResponse("No leave requests found for this employee"));

        return Ok(ApiResponse<List<LeaveRequestDto>>.SuccessResponse(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLeaveRequestDto dto)
    {
        await _service.CreateAsync(dto);
        return Ok(ApiResponse<string>.SuccessResponse(null, "Leave request created successfully"));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLeaveRequestDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return Ok(ApiResponse<string>.SuccessResponse(null, "Leave request updated successfully"));
    }

    [Authorize(Roles = "Admin, Manager")]
    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(Guid id)
    {
        await _service.ApproveAsync(id);
        return Ok(ApiResponse<string>.SuccessResponse(null, "Leave request approved successfully"));
    }

    [Authorize(Roles = "Admin, Manager")]
    [HttpPost("{id}/reject")]
    public async Task<IActionResult> Reject(Guid id)
    {
        await _service.RejectAsync(id);
        return Ok(ApiResponse<string>.SuccessResponse(null, "Leave request rejected successfully"));
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        await _service.CancelAsync(id);
        return Ok(ApiResponse<string>.SuccessResponse(null, "Leave request cancelled successfully"));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return Ok(ApiResponse<string>.SuccessResponse(null, "Leave request deleted successfully"));
    }
}