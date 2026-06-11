using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public interface IAttachmentRepository : IGenericRepository<AttendanceAttachment>
{
    Task<IReadOnlyList<AttendanceAttachment>> GetByRequestIdAsync(Guid requestId);
}