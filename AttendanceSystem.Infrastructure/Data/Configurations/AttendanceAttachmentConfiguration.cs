using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceSystem.Infrastructure;

public class AttendanceAttachmentConfiguration : IEntityTypeConfiguration<AttendanceAttachment>
{
    public void Configure(EntityTypeBuilder<AttendanceAttachment> builder)
    {
        builder.Property(x => x.AttendanceRequestId).IsRequired();

        builder.Property(x => x.FileName).IsRequired().HasMaxLength(255);

        builder.Property(x => x.FilePath).IsRequired().HasMaxLength(1000);

        builder.Property(x => x.FileSizeBytes).IsRequired();

        builder.HasOne(x => x.AttendanceRequest)
            .WithMany(x => x.Attachments)
            .HasForeignKey(x => x.AttendanceRequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}