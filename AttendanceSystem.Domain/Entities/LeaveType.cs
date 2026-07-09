namespace AttendanceSystem.Domain;

public class LeaveType : BaseEntity
{
    public string NameEnglish { get; private set; } = string.Empty;
    public string NameArabic { get; private set; } = string.Empty;
    public int DefaultDaysPerYear { get; private set; }
    public bool IsPaid { get; private set; }
    public bool RequiresApproval { get; private set; }

    public virtual ICollection<EmployeeLeaveBalance> Balances { get; private set; } = [];
    public virtual ICollection<LeaveRequest> LeaveRequests { get; private set; } = [];

    private LeaveType SetNameEnglish(string name) { NameEnglish = name.Trim(); return this; }
    private LeaveType SetNameArabic(string name) { NameArabic = name.Trim(); return this; }
    private LeaveType SetDefaultDays(int days) { DefaultDaysPerYear = days; return this; }
    private LeaveType SetIsPaid(bool isPaid) { IsPaid = isPaid; return this; }
    private LeaveType SetRequiresApproval(bool requiresApproval) { RequiresApproval = requiresApproval; return this; }

    public static LeaveType Create(string nameEn, string nameAr, int defaultDaysPerYear, bool isPaid, bool requiresApproval)
        => new LeaveType().ApplyData(nameEn, nameAr, defaultDaysPerYear, isPaid, requiresApproval);

    public LeaveType Update(string nameEn, string nameAr, int defaultDaysPerYear, bool isPaid, bool requiresApproval)
        => ApplyData(nameEn, nameAr, defaultDaysPerYear, isPaid, requiresApproval);

    private LeaveType ApplyData(string nameEn, string nameAr, int defaultDaysPerYear, bool isPaid, bool requiresApproval)
    {
        SetNameEnglish(nameEn).SetNameArabic(nameAr).SetDefaultDays(defaultDaysPerYear).SetIsPaid(isPaid).SetRequiresApproval(requiresApproval);
        return this;
    }
}