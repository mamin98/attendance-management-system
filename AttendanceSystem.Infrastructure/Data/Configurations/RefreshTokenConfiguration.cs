using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceSystem.Infrastructure;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {

        builder.Property(x => x.Token).HasMaxLength(128).IsRequired();

        builder.HasIndex(x => x.Token).IsUnique();

        builder.Property(x => x.EmployeeId).IsRequired();

        builder.Property(x => x.ExpiresAt).IsRequired();

        builder.Property(x => x.IsRevoked).HasDefaultValue(false);
    
        builder.HasOne(x => x.Employee)
            .WithMany(x => x.RefreshTokens)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}