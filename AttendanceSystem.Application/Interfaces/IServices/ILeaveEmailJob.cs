namespace AttendanceSystem.Application;

public interface ILeaveEmailJob
{
    Task SendLeaveStatusEmailAsync(Guid leaveRequestId, string status);
}