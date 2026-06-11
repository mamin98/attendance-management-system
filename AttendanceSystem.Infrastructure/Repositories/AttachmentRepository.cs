using Microsoft.EntityFrameworkCore;
using AttendanceSystem.Application;
using AttendanceSystem.Domain;

namespace AttendanceSystem.Infrastructure;

public class AttachmentRepository(AttendanceDbContext context)
    : GenericRepository<AttendanceAttachment>(context), IAttachmentRepository
{
    public async Task<IReadOnlyList<AttendanceAttachment>> GetByRequestIdAsync(Guid requestId)
        => await _context.AttendanceAttachments
            .AsNoTracking()
            .Where(x => x.AttendanceRequestId == requestId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
}