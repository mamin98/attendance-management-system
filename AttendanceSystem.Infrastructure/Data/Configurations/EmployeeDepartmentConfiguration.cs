using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceSystem.Infrastructure;

public class EmployeeDepartmentConfiguration : IEntityTypeConfiguration<EmployeeDepartment>
{
    public void Configure(EntityTypeBuilder<EmployeeDepartment> builder)
    {
        builder.Property(x => x.EmployeeId).IsRequired(false);

        builder.Property(x => x.DepartmentId).IsRequired(false);
        
        builder.HasKey(x => new { x.EmployeeId, x.DepartmentId });

        builder.HasOne(x => x.Employee)
            .WithMany(e => e.EmployeeDepartments)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Department)
            .WithMany(d => d.EmployeeDepartments)
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);


    }
}