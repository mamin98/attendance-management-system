using AttendanceSystem.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.API;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DepartmentsController(
    IDepartmentService service) : ControllerBase
{
    private readonly IDepartmentService _service = service;

    [HttpGet("paged")]
    public async Task<IActionResult> GetAllWithPagination(
        [FromQuery] DepartmentSearchDto search)
    {
        var result = await _service.GetAllWithPaginationAsync(search);

        return Ok(
            ApiResponse<PagedResult<DepartmentDto>>
                .SuccessResponse(result));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();

        return Ok(
            ApiResponse<List<DepartmentDto>>
                .SuccessResponse(result));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);

        if (result is null)
        {
            return NotFound(
                ApiResponse<string>.FailureResponse(
                    "Department not found"));
        }

        return Ok(
            ApiResponse<DepartmentDto>
                .SuccessResponse(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateDepartmentDto dto)
    {
        await _service.CreateAsync(dto);

        return Ok(
            ApiResponse<string>.SuccessResponse(
                null,
                "Department created successfully"));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateDepartmentDto dto)
    {
        await _service.UpdateAsync(id, dto);

        return Ok(
            ApiResponse<string>.SuccessResponse(
                null,
                "Department updated successfully"));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);

        return Ok(
            ApiResponse<string>.SuccessResponse(
                null,
                "Department deleted successfully"));
    }
}