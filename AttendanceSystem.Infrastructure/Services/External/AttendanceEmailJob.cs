using AttendanceSystem.Application;
using AttendanceSystem.Domain;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Infrastructure;

public class AttendanceEmailJob(
    IUnitOfWork unitOfWork,
    IEmailService emailService,
    ILogger<AttendanceEmailJob> logger) : IAttendanceEmailJob
{
    readonly IUnitOfWork _unitOfWork = unitOfWork;
    readonly IEmailService _emailService = emailService;
    readonly ILogger<AttendanceEmailJob> _logger = logger;

    public async Task SendRequestStatusEmailAsync(Guid requestId, string status)
    {
        AttendanceRequest? request = await _unitOfWork.AttendanceRequestRepository.GetByIdAsync(requestId);

        if (request?.Employee?.Email is null)
        {
            _logger.LogWarning("Skipped attendance status email — request {RequestId} or employee email not found", requestId);
            return;
        }

        await _emailService.SendAsync(
            request.Employee.Email,
            $"Attendance Request {status}",
            $"Your {request.RequestType} request for {request.RequestDate:dd/MM/yyyy} has been {status.ToLower()}.");
    }
}