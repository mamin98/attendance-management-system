using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceSystem.Infrastructure;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.Property(x => x.Role).HasConversion<int>().IsRequired();

        builder.Property(x => x.NameEnglish).HasMaxLength(200).IsRequired();

        builder.Property(x => x.NameArabic).HasMaxLength(200).IsRequired();

        builder.Property(x => x.Email).HasMaxLength(200).IsRequired();

        builder.HasIndex(x => x.Email).IsUnique();

    }
}