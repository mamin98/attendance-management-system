using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceSystem.Infrastructure;

public class SalaryStructureConfiguration : IEntityTypeConfiguration<SalaryStructure>
{
    public void Configure(EntityTypeBuilder<SalaryStructure> builder)
    {
        builder.Property(x => x.EmployeeId).IsRequired();
        builder.Property(x => x.BaseSalary).HasColumnType("decimal(12,2)").IsRequired();
        builder.Property(x => x.LateDeductionPerMinute).HasColumnType("decimal(12,2)").IsRequired();
        builder.Property(x => x.AbsenceDeductionPerDay).HasColumnType("decimal(12,2)").IsRequired();

        builder.HasIndex(x => x.EmployeeId).IsUnique();

        builder.HasOne(x => x.Employee)
            .WithOne(e => e.SalaryStructure)
            .HasForeignKey<SalaryStructure>(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}