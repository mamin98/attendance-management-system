using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public class MonthlyAttendanceReportDto
{
    public EmployeeSimpleDto? EmployeeData { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }

    public int WorkingDaysInMonth { get; set; }
    public int PresentDays { get; set; }
    public int AbsentDays { get; set; }
    public int ApprovedLeaveDays { get; set; }
    public int ApprovedRemoteDays { get; set; }
    public int ApprovedPermissionDays { get; set; }

    public decimal TotalWorkedHours { get; set; }
    public int LateDaysCount { get; set; }
    public int TotalLateMinutes { get; set; }
    public int EarlyLeaveDaysCount { get; set; }
    public int TotalEarlyLeaveMinutes { get; set; }

    public bool LatePolicyExceeded { get; set; }
    public bool EarlyLeavePolicyExceeded { get; set; }

    public List<DailyAttendanceDetailDto> Days { get; set; } = [];
}

public class DailyAttendanceDetailDto
{
    public DateTime Date { get; set; }
    public bool IsWorkingDay { get; set; }
    public string? ExpectedStart { get; set; } = string.Empty;
    public string? ExpectedEnd { get; set; } = string.Empty;
    public string? ActualCheckIn { get; set; } = string.Empty;
    public string? ActualCheckOut { get; set; } = string.Empty;
    public int LateMinutes { get; set; }
    public int EarlyLeaveMinutes { get; set; }
    public decimal WorkedHours { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class GenerateReportRequestDto
{
    public int Month { get; set; }
    public int Year { get; set; }
    public Guid? EmployeeId { get; set; }
    public Guid? DepartmentId { get; set; }
}