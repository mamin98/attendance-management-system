namespace AttendanceSystem.Application;

public class DepartmentAssignmentDto
{
    public Guid DepartmentId { get; set; }
    public string StartDate { get; set; } = string.Empty;
    public string? EndDate  { get; set; }
}

public class AssignDepartmentsDto
{
    public Guid EmployeeId { get; set; }
    public List<DepartmentAssignmentDto> Departments { get; set; } = [];
}