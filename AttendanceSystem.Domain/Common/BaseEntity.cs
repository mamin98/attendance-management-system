namespace AttendanceSystem.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? LastModifiedAt { get; set; }
    public string LastModifiedBy { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;

    public void SoftDelete() => IsDeleted = true;    
}