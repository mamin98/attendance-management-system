using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceSystem.Infrastructure;

public class PayrollRecordConfiguration : IEntityTypeConfiguration<PayrollRecord>
{
    public void Configure(EntityTypeBuilder<PayrollRecord> builder)
    {
        builder.Property(x => x.EmployeeId).IsRequired();
        builder.Property(x => x.Month).IsRequired();
        builder.Property(x => x.Year).IsRequired();
        builder.Property(x => x.BaseSalary).HasColumnType("decimal(12,2)").IsRequired();
        builder.Property(x => x.LateDeductionAmount).HasColumnType("decimal(12,2)").IsRequired();
        builder.Property(x => x.AbsenceDeductionAmount).HasColumnType("decimal(12,2)").IsRequired();
        builder.Property(x => x.TotalDeductions).HasColumnType("decimal(12,2)").IsRequired();
        builder.Property(x => x.NetSalary).HasColumnType("decimal(12,2)").IsRequired();

        builder.HasOne(x => x.Employee)
            .WithMany(e => e.PayrollRecords)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}