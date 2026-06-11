namespace AttendanceSystem.Domain;

public class AttendanceAttachment : BaseEntity
{
    public Guid AttendanceRequestId { get; private set; }
    public string FileName { get; private set; } = string.Empty;
    public string FilePath { get; private set; } = string.Empty;
    public long FileSizeBytes { get; private set; }
    public virtual AttendanceRequest? AttendanceRequest { get; private set; }

    public static AttendanceAttachment Create(Guid requestId, string fileName, string filePath, long size)
        => new() { AttendanceRequestId = requestId, FileName = fileName, FilePath = filePath, FileSizeBytes = size };
}