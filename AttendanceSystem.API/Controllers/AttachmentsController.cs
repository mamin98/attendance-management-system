using AttendanceSystem.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.API;

[Authorize]
[ApiController]
[Route("api/attendance-requests/{requestId}/attachments")]
public class AttachmentsController(IAttachmentService attachmentService) : ControllerBase
{
    readonly IAttachmentService _attachmentService = attachmentService;

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid requestId)
    {
        List<AttachmentDto> result = await _attachmentService.GetByRequestIdAsync(requestId);

        if (result.Count == 0)
            return NotFound(ApiResponse<string>.FailureResponse("No attachments found"));

        return Ok(ApiResponse<List<AttachmentDto>>.SuccessResponse(result));
    }

    [HttpPost]
    public async Task<IActionResult> Upload(Guid requestId, IFormFile file)
    {
        await _attachmentService.AddAsync(requestId, file);
        return Ok(ApiResponse<string>.SuccessResponse(null, "Attachment uploaded successfully"));
    }

    [HttpGet("{attachmentId}")]
    public async Task<IActionResult> Download(Guid requestId, Guid attachmentId)
    {
        var (stream, fileName) = await _attachmentService.GetFileAsync(attachmentId);
        return File(stream, "application/octet-stream", fileName);
    }

    [HttpDelete("{attachmentId}")]
    public async Task<IActionResult> Delete(Guid requestId, Guid attachmentId)
    {
        await _attachmentService.DeleteAsync(attachmentId);
        return Ok(ApiResponse<string>.SuccessResponse(null, "Attachment deleted successfully"));
    }
}