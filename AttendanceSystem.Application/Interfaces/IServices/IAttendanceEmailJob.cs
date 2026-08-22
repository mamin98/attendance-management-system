namespace AttendanceSystem.Application;

public interface IAttendanceEmailJob
{
    Task SendRequestStatusEmailAsync(Guid requestId, string status);
}