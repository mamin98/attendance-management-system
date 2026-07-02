
using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceSystem.Infrastructure;

public class ShiftDayDetailConfiguration : IEntityTypeConfiguration<ShiftDayDetail>
{
    public void Configure(EntityTypeBuilder<ShiftDayDetail> builder)
    {            
        builder.Property(x => x.ShiftId);
        builder.Property(x => x.ShiftDayId);
        builder.Property(x => x.From).IsRequired();
        builder.Property(x => x.To).IsRequired();

        builder
            .HasOne(c => c.Shift)
            .WithMany(nameof(Shift.ShiftDayDetails))
            .HasForeignKey(c => c.ShiftId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(c => c.ShiftDay)
            .WithMany(nameof(ShiftDay.ShiftDayDetails))
            .HasForeignKey(c => c.ShiftDayId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}