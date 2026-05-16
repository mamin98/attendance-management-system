using AttendanceSystem.Application;
using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace AttendanceSystem.Infrastructure;

public class AttendanceDbContext : DbContext
{
    private readonly ICurrentUserService _currentUserService;
    public AttendanceDbContext(DbContextOptions<AttendanceDbContext> options,
    ICurrentUserService currentUserService)
    : base(options)
    {
        _currentUserService = currentUserService;
    }

    // DbSets    
    public DbSet<AttendanceRequest> AttendanceRequests { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<EmployeeDepartment> EmployeeDepartments { get; set; }

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

            var method = typeof(AttendanceDbContext)
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
               .IsRequired(false);

           entity.Property(x => x.NameEnglish)
               .HasMaxLength(200)
               .IsRequired();

           entity.Property(x => x.NameArabic)
               .HasMaxLength(200)
               .IsRequired();
       });

         modelBuilder.Entity<EmployeeDepartment>(entity =>
       {
           entity.Property(x => x.EmployeeId)
               .IsRequired(false);

            entity.Property(x => x.DepartmentId)
               .IsRequired(false);           
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

        modelBuilder.Entity<EmployeeDepartment>()
            .HasKey(x => new { x.EmployeeId, x.DepartmentId });

        modelBuilder.Entity<EmployeeDepartment>()
            .HasOne(x => x.Employee)
            .WithMany(e => e.EmployeeDepartments)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EmployeeDepartment>()
            .HasOne(x => x.Department)
            .WithMany(d => d.EmployeeDepartments)
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public override async Task<int> SaveChangesAsync(
    CancellationToken cancellationToken = default)
    {
        ApplyAuditInformation();

        return await base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditInformation()
    {
        var entries = ChangeTracker
            .Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;

                entry.Entity.CreatedBy =
                    _currentUserService.UserEmail;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.LastModifiedAt =
                    DateTime.UtcNow;

                entry.Entity.LastModifiedBy =
                    _currentUserService.UserEmail;
            }
        }
    }
}