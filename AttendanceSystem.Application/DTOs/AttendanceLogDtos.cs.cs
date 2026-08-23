namespace AttendanceSystem.Application;

public class AttendanceLogDto
{
    public Guid Id { get; set; }
    public EmployeeSimpleDto? EmployeeData { get; set; }
    public DateTime Date { get; set; }
    public TimeOnly? CheckIn { get; set; }
    public TimeOnly? CheckOut { get; set; }
    public string Source { get; set; } = string.Empty;
}

public class CreateAttendanceLogDto
{
    public Guid EmployeeId { get; set; }
    public string Date { get; set; } = string.Empty;
    public string? CheckIn { get; set; } = string.Empty;
    public string? CheckOut { get; set; } = string.Empty;
}

public class UpdateAttendanceLogDto
{
    public Guid Id { get; set; }
    public string? CheckIn { get; set; } = string.Empty;
    public string? CheckOut { get; set; } = string.Empty;
}

public class AttendanceLogSearchDto : SearchDto
{
    public Guid? EmployeeId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
public class ImportAttendanceRowResultDto
{
    public int RowNumber { get; set; }
    public string? EmployeeEmail { get; set; }
    public string? Error { get; set; }
}

public class ImportAttendanceResultDto
{
    public int TotalRows { get; set; }
    public int SuccessCount { get; set; }
    public int FailedCount { get; set; }
    public List<ImportAttendanceRowResultDto> Errors { get; set; } = [];
}