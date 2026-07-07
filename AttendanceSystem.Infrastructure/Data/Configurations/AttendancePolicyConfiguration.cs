using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceSystem.Infrastructure;

public class AttendancePolicyConfiguration : IEntityTypeConfiguration<AttendancePolicy>
{
    public void Configure(EntityTypeBuilder<AttendancePolicy> builder)
    {
        builder.Property(x => x.NameEnglish).HasMaxLength(200).IsRequired();
        builder.Property(x => x.NameArabic).HasMaxLength(200).IsRequired();
        builder.Property(x => x.MaxLateMinutesPerMonth).IsRequired();
        builder.Property(x => x.MaxPermissionsPerMonth).IsRequired();
        builder.Property(x => x.MaxRemoteDaysPerMonth).IsRequired();
        builder.Property(x => x.MaxEarlyLeavesPerMonth).IsRequired();
        builder.Property(x => x.RequiresManagerApproval).HasDefaultValue(false);    
    }
}