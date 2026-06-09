using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceSystem.Infrastructure;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.Property(x => x.ManagerId).IsRequired(false);

        builder.Property(x => x.NameEnglish).HasMaxLength(200).IsRequired();

        builder.Property(x => x.NameArabic).HasMaxLength(200).IsRequired();

        builder.HasOne(d => d.Manager)
            .WithMany(e => e.DepartmentManagers)
            .HasForeignKey(d => d.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}