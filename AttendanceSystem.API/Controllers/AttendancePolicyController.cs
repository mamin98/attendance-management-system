using AttendanceSystem.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.API;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AttendancePolicyController(IAttendancePolicyService service) : ControllerBase
{
    readonly IAttendancePolicyService _service = service;

    [HttpGet("paged")]
    public async Task<IActionResult> GetAllWithPagination([FromQuery] AttendancePolicySearchDto search)
    {
        PagedResult<AttendancePolicyDto> result = await _service.GetAllWithPaginationAsync(search);

        if (result.TotalCount == 0)
            return NotFound(ApiResponse<string>.FailureResponse("No policies found"));

        return Ok(ApiResponse<PagedResult<AttendancePolicyDto>>.SuccessResponse(result));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<AttendancePolicyDto> result = await _service.GetAllAsync();

        if (result.Count == 0)
            return NotFound(ApiResponse<string>.FailureResponse("No policies found"));

        return Ok(ApiResponse<List<AttendancePolicyDto>>.SuccessResponse(result));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        AttendancePolicyDto? result = await _service.GetByIdAsync(id);

        if (result is null)
            return NotFound(ApiResponse<string>.FailureResponse("Policy not found"));

        return Ok(ApiResponse<AttendancePolicyDto>.SuccessResponse(result));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAttendancePolicyDto dto)
    {
        await _service.CreateAsync(dto);
        return Ok(ApiResponse<string>.SuccessResponse(null, "Policy created successfully"));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAttendancePolicyDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return Ok(ApiResponse<string>.SuccessResponse(null, "Policy updated successfully"));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return Ok(ApiResponse<string>.SuccessResponse(null, "Policy deleted successfully"));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("assign-department")]
    public async Task<IActionResult> AssignToDepartment([FromBody] AssignPolicyToDepartmentDto dto)
    {
        await _service.AssignToDepartmentAsync(dto);
        return Ok(ApiResponse<string>.SuccessResponse(null, "Policy assigned to department successfully"));
    }
}