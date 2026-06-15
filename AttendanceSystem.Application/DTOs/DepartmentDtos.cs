namespace AttendanceSystem.Application;

public class DepartmentDto
{
    public Guid Id { get; set; }
    public string NameEnglish { get; set; } = string.Empty;
    public string NameArabic { get; set; } = string.Empty;
    public EmployeeSimpleDto? ManagerData { get; set; }
    public int EmployeeCount { get; set; }
}

public class DepartmentSimpleDto
{
    public Guid Id { get; set; }
    public string NameEnglish { get; set; } = string.Empty;
    public string NameArabic { get; set; } = string.Empty;
}

public class CreateDepartmentDto
{
    public string NameEnglish { get; set; } = string.Empty;
    public string NameArabic { get; set; } = string.Empty;
    public Guid? ManagerId { get; set; }
}

public class UpdateDepartmentDto : CreateDepartmentDto
{
    public Guid Id { get; set; }
}


public class DepartmentSearchDto : SearchDto
{
    public Guid? ManagerId { get; set; }
}