namespace AttendanceSystem.Domain;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } 
    public string CreatedBy { get; set; } = string.Empty; // current user
    public DateTime? LastModifiedAt { get; set; }
    public string LastModifiedBy { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;

    public void SetId(Guid id) => Id = id;
    public void SoftDelete() => IsDeleted = true;    
}