using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceSystem.Infrastructure;

public class LeaveTypeConfiguration : IEntityTypeConfiguration<LeaveType>
{
    public void Configure(EntityTypeBuilder<LeaveType> builder)
    {
        builder.Property(x => x.NameEnglish).HasMaxLength(200).IsRequired();
        builder.Property(x => x.NameArabic).HasMaxLength(200).IsRequired();
        builder.Property(x => x.DefaultDaysPerYear).IsRequired();
        builder.Property(x => x.IsPaid).HasDefaultValue(true);
        builder.Property(x => x.RequiresApproval).HasDefaultValue(true);
    }
}