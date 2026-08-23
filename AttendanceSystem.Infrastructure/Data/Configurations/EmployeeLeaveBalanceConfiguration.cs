using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceSystem.Infrastructure;

public class EmployeeLeaveBalanceConfiguration : IEntityTypeConfiguration<EmployeeLeaveBalance>
{
    public void Configure(EntityTypeBuilder<EmployeeLeaveBalance> builder)
    {
        builder.Property(x => x.EmployeeId).IsRequired();
        builder.Property(x => x.LeaveTypeId).IsRequired();
        builder.Property(x => x.Year).IsRequired();
        builder.Property(x => x.AllocatedDays).HasColumnType("decimal(5,1)").IsRequired();
        builder.Property(x => x.UsedDays).HasColumnType("decimal(5,1)").IsRequired();
        builder.Ignore(x => x.RemainingDays);

        builder.HasIndex(x => new { x.EmployeeId, x.LeaveTypeId, x.Year }).IsUnique();

        builder.HasOne(x => x.Employee)
            .WithMany(e => e.LeaveBalances)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.LeaveType)
            .WithMany(t => t.Balances)
            .HasForeignKey(x => x.LeaveTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}