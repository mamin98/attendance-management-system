namespace AttendanceSystem.Domain;

public class Employee : BaseEntity
{
    public string NameEnglish { get; private set; } = string.Empty;
    public string NameArabic { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public EmployeeRole Role { get; private set; }

    public virtual ICollection<EmployeeDepartment> EmployeeDepartments { get; private set; } = [];
    public virtual ICollection<AttendanceRequest> AttendanceRequests { get; private set; } = [];
    public virtual ICollection<Department> DepartmentManagers { get; private set; } = [];
    public virtual ICollection<RefreshToken> RefreshTokens { get; private set; } = [];
    public virtual ICollection<EmployeeShift> EmployeeShifts { get; private set; } = [];
    public virtual ICollection<EmployeeLeaveBalance> LeaveBalances { get; private set; } = [];
    public virtual ICollection<LeaveRequest> LeaveRequests { get; private set; } = [];

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

    private Employee SetPasswordHash(string passwordHash)
    {
        PasswordHash = passwordHash;
        return this;
    }

    public Employee ChangePassword(string passwordHash)
    => SetPasswordHash(passwordHash);

    private Employee SetRole(EmployeeRole role)
    {
        Role = role;
        return this;
    }

    public static Employee Create(
        EmployeeRole role,
        string nameEn,
        string nameAr,
        string email,
        string passwordHash)
    {
        return new Employee()
           .ApplyData(role, nameEn, nameAr, email)
           .SetPasswordHash(passwordHash);

    }

    public Employee Update(
        EmployeeRole role,
        string nameEn,
        string nameAr,
        string email)
        => ApplyData(role, nameEn, nameAr, email);


    private Employee ApplyData(
        EmployeeRole role,
        string nameEn,
        string nameAr,
        string email)
    {
        SetRole(role)
        .SetNameEnglish(nameEn)
        .SetNameArabic(nameAr)
        .SetEmail(email);

        return this;
    }

}