using AttendanceSystem.Application;
using AttendanceSystem.Domain;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Infrastructure;

public class LeaveEmailJob(
    IUnitOfWork unitOfWork,
    IEmailService emailService,
    ILogger<LeaveEmailJob> logger) : ILeaveEmailJob
{
    readonly IUnitOfWork _unitOfWork = unitOfWork;
    readonly IEmailService _emailService = emailService;
    readonly ILogger<LeaveEmailJob> _logger = logger;

    public async Task SendLeaveStatusEmailAsync(Guid leaveRequestId, string status)
    {
        LeaveRequest? request = await _unitOfWork.LeaveRequestRepository.GetByIdAsync(leaveRequestId);

        if (request?.Employee?.Email is null)
        {
            _logger.LogWarning("Skipped leave status email — request {RequestId} or employee email not found", leaveRequestId);
            return;
        }

        await _emailService.SendAsync(
            request.Employee.Email,
            $"Leave Request {status}",
            $"Your leave request from {request.StartDate:dd/MM/yyyy} to {request.EndDate:dd/MM/yyyy} has been {status.ToLower()}.");
    }
}