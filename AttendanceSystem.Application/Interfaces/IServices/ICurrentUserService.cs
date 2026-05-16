namespace AttendanceSystem.Application;

public interface ICurrentUserService
{
    string UserId { get; }

    string UserEmail { get; }

    string UserRole { get; }

    bool IsAuthenticated { get; }
}