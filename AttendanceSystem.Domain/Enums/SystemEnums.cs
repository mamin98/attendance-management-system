namespace AttendanceSystem.Domain.Enums;
public enum RequestType
{
    Late = 1,
    Remote,
    Permission,
    EarlyLeave
}

public enum RequestStatus
{
    Pending = 1,
    Approved,
    Rejected,
    Cancelled
}

public enum EmployeeRole
{
    Employee = 1,
    Manager,
    Admin
}
