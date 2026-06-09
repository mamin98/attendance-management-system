using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceSystem.Infrastructure;

public class AttendanceRequestConfiguration : IEntityTypeConfiguration<AttendanceRequest>
{
    public void Configure(EntityTypeBuilder<AttendanceRequest> builder)
    {
            builder.Property(x => x.RequestType).HasConversion<int>().IsRequired();

            builder.Property(x => x.RequestStatus).HasConversion<int>().IsRequired();

            builder.Property(x => x.RequestDate).IsRequired();

            builder.Property(x => x.EmployeeId).IsRequired();

            builder.Property(x => x.Reason).HasMaxLength(500);

            builder.Property(x => x.FromTime);
            builder.Property(x => x.ToTime);        

            builder.HasOne(x => x.Employee)
                .WithMany(x => x.AttendanceRequests)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

    }
}