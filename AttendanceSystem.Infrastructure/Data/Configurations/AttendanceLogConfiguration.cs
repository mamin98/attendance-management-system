using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceSystem.Infrastructure;

public class AttendanceLogConfiguration : IEntityTypeConfiguration<AttendanceLog>
{
    public void Configure(EntityTypeBuilder<AttendanceLog> builder)
    {
        builder.Property(x => x.EmployeeId).IsRequired();
        builder.Property(x => x.Date).IsRequired();
        builder.Property(x => x.Source).HasMaxLength(50);

        builder.HasIndex(x => new { x.EmployeeId, x.Date }).IsUnique();

        builder.HasOne(x => x.Employee)
            .WithMany(e => e.AttendanceLogs)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}