using AttendanceSystem.Domain.Common;

namespace AttendanceSystem.Domain.Entities;

public class Department : BaseEntity
{
    public string NameEnglish { get; private set; } = string.Empty;
    public string NameArabic { get; private set; } = string.Empty;
    public Guid ManagerId { get; private set; }
    public virtual Employee? Manager { get; private set; }

    public virtual ICollection<Employee> Employees { get; private set; } = [];

    private Department SetManagerId(Guid managerId)
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

    public static Department Create(
    Guid managerId,
    string nameEn,
    string nameAr)
    => new Department()
            .ApplyData(managerId, nameEn, nameAr);

    public Department Update(
        Guid managerId,
        string nameEn,
        string nameAr)
        => ApplyData(managerId, nameEn, nameAr);
    
    private Department ApplyData(
        Guid managerId,
        string nameEn,
        string nameAr)
    {
        SetManagerId(managerId)
        .SetNameEnglish(nameEn)
        .SetNameArabic(nameAr);

        return this;
    }
}