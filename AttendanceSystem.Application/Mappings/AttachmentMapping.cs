using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public static class AttachmentMapping
{
    public static AttachmentDto ToDto(this AttendanceAttachment entity)
        => new()
        {
            Id = entity.Id,
            FileName = entity.FileName,
            FileSizeBytes = entity.FileSizeBytes,
            CreatedAt = entity.CreatedAt,
        };
}