namespace AttendanceSystem.Domain;

public class AttendancePolicy : BaseEntity
{
    public string NameEnglish { get; private set; } = string.Empty;
    public string NameArabic { get; private set; } = string.Empty;
    public int MaxLateMinutesPerMonth { get; private set; }
    public int MaxPermissionsPerMonth { get; private set; }
    public int MaxRemoteDaysPerMonth { get; private set; }
    public int MaxEarlyLeavesPerMonth { get; private set; }
    public bool RequiresManagerApproval { get; private set; }

    public virtual ICollection<Department> Departments { get; private set; } = [];

    private AttendancePolicy SetNameEnglish(string name) { NameEnglish = name.Trim(); return this; }
    private AttendancePolicy SetNameArabic(string name) { NameArabic = name.Trim(); return this; }

    private AttendancePolicy SetLimits(int maxLate, int maxPermissions, int maxRemote, int maxEarlyLeaves)
    {
        MaxLateMinutesPerMonth = maxLate;
        MaxPermissionsPerMonth = maxPermissions;
        MaxRemoteDaysPerMonth = maxRemote;
        MaxEarlyLeavesPerMonth = maxEarlyLeaves;
        return this;
    }

    private AttendancePolicy SetApprovalRule(bool requiresApproval) { RequiresManagerApproval = requiresApproval; return this; }

    public static AttendancePolicy Create(
        string nameEn, string nameAr, int maxLate, int maxPermissions, int maxRemote, int maxEarlyLeaves, bool requiresApproval)
        => new AttendancePolicy().ApplyData(nameEn, nameAr, maxLate, maxPermissions, maxRemote, maxEarlyLeaves, requiresApproval);

    public AttendancePolicy Update(
        string nameEn, string nameAr, int maxLate, int maxPermissions, int maxRemote, int maxEarlyLeaves, bool requiresApproval)
        => ApplyData(nameEn, nameAr, maxLate, maxPermissions, maxRemote, maxEarlyLeaves, requiresApproval);

    private AttendancePolicy ApplyData(
        string nameEn, string nameAr, int maxLate, int maxPermissions, int maxRemote, int maxEarlyLeaves, bool requiresApproval)
    {
        SetNameEnglish(nameEn).SetNameArabic(nameAr).SetLimits(maxLate, maxPermissions, maxRemote, maxEarlyLeaves).SetApprovalRule(requiresApproval);
        return this;
    }
}