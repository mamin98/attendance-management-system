namespace AttendanceSystem.Domain;

public class Employee : BaseEntity
{
    public string NameEnglish { get; private set; } = string.Empty;
    public string NameArabic { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public Guid DepartmentId { get; private set; }
    public EmployeeRole Role { get; private set; }
    
    public virtual Department? Department { get; private set; }
    public virtual ICollection<AttendanceRequest> AttendanceRequests { get; private set; } = [];
    public virtual ICollection<Department> DepartmentManagers { get; private set; } = [];

    private Employee SetDepartmentId(Guid departmentId)
    {
        DepartmentId = departmentId;
        return this;
    }

    private Employee SetNameEnglish(string name)
    {
        NameEnglish = name.Trim();
        return this;
    }


    private Employee SetNameArabic(string name)
    {
        NameArabic = name.Trim();
        return this;
    }

    private Employee SetEmail(string email)
    {
        Email = email.Trim().ToLower();
        return this;
    }

    private Employee SetRole(EmployeeRole role)
    {
        Role = role;
        return this;
    }
   
    public static Employee Create(
        Guid departmentId,
        EmployeeRole role,
        string nameEn,
        string nameAr,
        string email)
        => new Employee()
            .ApplyData(departmentId, role, nameEn, nameAr, email);

    public Employee Update(
        Guid departmentId,
        EmployeeRole role,
        string nameEn,
        string nameAr,
        string email)
        => ApplyData(departmentId, role, nameEn, nameAr, email);
    

    private Employee ApplyData(
        Guid departmentId,
        EmployeeRole role,
        string nameEn,
        string nameAr,
        string email)
    {
        SetDepartmentId(departmentId)
        .SetRole(role)
        .SetNameEnglish(nameEn)
        .SetNameArabic(nameAr)
        .SetEmail(email);

        return this;
    }

}