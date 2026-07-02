
using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceSystem.Infrastructure;

public class ShiftDayConfiguration : IEntityTypeConfiguration<ShiftDay>
{
    public void Configure(EntityTypeBuilder<ShiftDay> builder)
    {            
        builder.Property(x => x.ShiftId);
        builder.Property(x => x.Day).IsRequired();
        builder.Property(x => x.IsOffDay).IsRequired();

        builder
            .HasOne(c => c.Shift)
            .WithMany(nameof(Shift.ShiftDays))
            .HasForeignKey(c => c.ShiftId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}