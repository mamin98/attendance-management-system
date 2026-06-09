namespace AttendanceSystem.Domain;

public class RefreshToken : BaseEntity
{
    public string Token { get; private set; } = string.Empty;
    public Guid EmployeeId { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsRevoked { get; private set; } = false;
    public virtual Employee? Employee { get; private set; }


    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive  => !IsRevoked && !IsExpired;
    public void Revoke() => IsRevoked = true;

    public static RefreshToken Create(Guid employeeId, RefreshTokenExpiry expiry = RefreshTokenExpiry.SevenDays)
        => new()
        {
            Token = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N"),
            EmployeeId = employeeId,
            ExpiresAt = DateTime.UtcNow.AddDays((int)expiry)
        };
}