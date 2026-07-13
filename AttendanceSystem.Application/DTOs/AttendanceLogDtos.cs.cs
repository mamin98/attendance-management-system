namespace AttendanceSystem.Application;

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