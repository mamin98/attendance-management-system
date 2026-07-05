namespace AttendanceSystem.Domain;

public class Department : BaseEntity
{
    public string NameEnglish { get; private set; } = string.Empty;
    public string NameArabic { get; private set; } = string.Empty;
    public Guid? ManagerId { get; private set; }
    public virtual Employee? Manager { get; private set; }
    public virtual ICollection<EmployeeDepartment> EmployeeDepartments { get; private set; } = [];
    public Guid? PolicyId { get; private set; }
    public virtual AttendancePolicy? Policy { get; private set; }


    public Department SetManagerId(Guid? managerId)
    {
        ManagerId = managerId;
        return this;
    }

    private Department SetNameEnglish(string name)
    {
        NameEnglish = name.Trim();
        return this;
    }

    private Department SetNameArabic(string name)
    {
        NameArabic = name.Trim();
        return this;
    }
    public Department SetPolicyId(Guid? policyId)
    {
        PolicyId = policyId;
        return this;
    }

    public static Department Create(
    Guid? managerId,
    Guid? policyId,
    string nameEn,
    string nameAr)
    => new Department()
            .ApplyData(managerId, policyId, nameEn, nameAr);

    public Department Update(
        Guid? managerId,
    Guid? policyId,
        string nameEn,
        string nameAr)
        => ApplyData(managerId, policyId, nameEn, nameAr);

    private Department ApplyData(
        Guid? managerId,
        Guid? policyId,
        string nameEn,
        string nameAr)
    {
        SetManagerId(managerId)
        .SetPolicyId(policyId)
        .SetNameEnglish(nameEn)
        .SetNameArabic(nameAr);

        return this;
    }
}