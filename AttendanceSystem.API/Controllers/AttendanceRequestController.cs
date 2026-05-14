using AttendanceSystem.Application;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.API;

[ApiController]
[Route("api/[controller]")]
public class AttendanceRequestsController : ControllerBase
{
    private readonly IAttendanceRequestService _service;

    public AttendanceRequestsController(IAttendanceRequestService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAllWithPagination(int page = 1, int pageSize = 10)
    {
        PagedResult<AttendanceRequestDto> result = await _service.GetAllWithPaginationAsync(page, pageSize);
        return Ok(result);
    }

    public async Task<IActionResult> GetAll()
    {
        List<AttendanceRequestDto> result = await _service.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        AttendanceRequestDto? result = await _service.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAttendanceRequestDto dto)
    {
        await _service.CreateAsync(dto);
        return Ok("Created");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateAttendanceRequestDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return Ok("Updated");
    }

    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(Guid id)
    {
        await _service.ApproveAsync(id);
        return Ok("Approved");
    }

    [HttpPost("{id}/reject")]
    public async Task<IActionResult> Reject(Guid id)
    {
        await _service.RejectAsync(id);
        return Ok("Rejected");
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        await _service.CancelAsync(id);
        return Ok("Canceled");

    }
}