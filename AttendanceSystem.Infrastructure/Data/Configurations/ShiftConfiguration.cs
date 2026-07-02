using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceSystem.Infrastructure;

public class ShiftConfiguration : IEntityTypeConfiguration<Shift>
{
    public void Configure(EntityTypeBuilder<Shift> builder)
    {
        builder.Property(x => x.NameEnglish).HasMaxLength(100).IsRequired();
        builder.Property(x => x.NameArabic).HasMaxLength(100).IsRequired();
        builder.Property(x => x.StartDate).IsRequired(false);
        builder.Property(x => x.EndDate).IsRequired(false);
        builder.Property(x => x.GracePeriodMinutes).IsRequired();
    }
}