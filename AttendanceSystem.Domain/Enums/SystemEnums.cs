namespace AttendanceSystem.Domain;

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


public enum RefreshTokenExpiry
{
    SevenDays    = 7,
    FifteenDays  = 15,
    ThirtyDays   = 30,
    NinetyDays   = 90
}