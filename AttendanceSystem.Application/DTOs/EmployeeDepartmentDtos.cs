namespace AttendanceSystem.Application;

public class EmployeeDepartmentDto
{
    public Guid DepartmentId { get; set; }
    public string NameEnglish { get; set; } = string.Empty;
    public string NameArabic { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
}

public class DepartmentAssignmentDto
{
    public Guid DepartmentId { get; set; }
    public string StartDate { get; set; } = string.Empty;
    public string? EndDate { get; set; }
}

public class AssignDepartmentsDto
{
    public Guid EmployeeId { get; set; }
    public List<DepartmentAssignmentDto> Departments { get; set; } = [];
}