using AttendanceSystem.Domain.Common;
using AttendanceSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AttendanceSystem.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // DbSets    
    public DbSet<AttendanceRequest> AttendanceRequests { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }

    // Model Configurations    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureBaseEntity(modelBuilder);

        ApplyGlobalFilters(modelBuilder);

        ConfigureEntities(modelBuilder);

        ConfigureRelationships(modelBuilder);
    }

    // Base Entity Configuration    
    private static void ConfigureBaseEntity(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                continue;

            modelBuilder.Entity(entityType.ClrType, entity =>
            {
                entity.HasKey(nameof(BaseEntity.Id));

                entity.Property(nameof(BaseEntity.CreatedAt))
                    .IsRequired();

                entity.Property(nameof(BaseEntity.CreatedBy))
                    .HasMaxLength(100);

                entity.Property(nameof(BaseEntity.LastModifiedBy))
                    .HasMaxLength(100);

                entity.Property(nameof(BaseEntity.IsDeleted))
                    .HasDefaultValue(false);
            });
        }
    }

    // Soft Delete Global Filters    
    private static void ApplyGlobalFilters(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                continue;

            var method = typeof(AppDbContext)
                .GetMethod(nameof(SetSoftDeleteFilter),
                    BindingFlags.NonPublic | BindingFlags.Static)!
                .MakeGenericMethod(entityType.ClrType);

            method.Invoke(null, new object[] { modelBuilder });
        }
    }

    private static void SetSoftDeleteFilter<TEntity>(ModelBuilder builder)
        where TEntity : BaseEntity
    {
        builder.Entity<TEntity>()
            .HasQueryFilter(x => !x.IsDeleted);
    }

    // Entity Configurations    
    private static void ConfigureEntities(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AttendanceRequest>(entity =>
        {
            entity.Property(x => x.RequestType)
                .HasConversion<int>()
                .IsRequired();

            entity.Property(x => x.RequestStatus)
                .HasConversion<int>()
                .IsRequired();

            entity.Property(x => x.RequestDate)
                .IsRequired();

            entity.Property(x => x.EmployeeId)
                .IsRequired();

            entity.Property(x => x.Reason)
                .HasMaxLength(500);

            entity.Property(x => x.FromTime);

            entity.Property(x => x.ToTime);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(x => x.DepartmentId)
                .IsRequired();

            entity.Property(x => x.Role)
                .HasConversion<int>()
                .IsRequired();

            entity.Property(x => x.NameEnglish)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.NameArabic)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.Email)
                .HasMaxLength(200)
                .IsRequired();

            entity.HasIndex(x => x.Email)
                .IsUnique();
        });

         modelBuilder.Entity<Department>(entity =>
        {
            entity.Property(x => x.ManagerId)
                .IsRequired();         

            entity.Property(x => x.NameEnglish)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.NameArabic)
                .HasMaxLength(200)
                .IsRequired();         
        });
    }

    // Relationships    
    private static void ConfigureRelationships(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AttendanceRequest>()
            .HasOne(x => x.Employee)
            .WithMany(x => x.AttendanceRequests)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Department>()
            .HasOne(d => d.Manager)
            .WithMany(e => e.DepartmentManagers)
            .HasForeignKey(d => d.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Employee>()
            .HasOne(x => x.Department)
            .WithMany(x => x.Employees)
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }

}