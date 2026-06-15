using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public class EmployeeDto : EmployeeDataDto
{
    public Guid Id { get; set; }
    public List<DepartmentSimpleDto> Departments { get; set; } = [];
}



public class CreateEmployeeDto : EmployeeDataDto
{
    public string Password { get; set; } = string.Empty;
    public List<Guid> DepartmentIds { get; set; } = [];
}

public class UpdateEmployeeDto : EmployeeDataDto
{
    public Guid Id { get; set; }
    public List<Guid> DepartmentIds { get; set; } = [];
}

public class EmployeeSearchDto : SearchDto
{
    public EmployeeRole? Role { get; set; }
    public Guid? DepartmentId { get; set; }
    public bool? IsActive { get; set; }
}

public class EmployeeDataDto
{
    public string NameArabic { get; set; } = string.Empty;
    public required string NameEnglish { get; set; }
    public string Email { get; set; } = string.Empty;
    public EmployeeRole Role { get; set; }

}
public class EmployeeSimpleDto
{
    public Guid Id { get; set; }
    public required string NameEnglish { get; set; }
    public required string NameArabic { get; set; }
}