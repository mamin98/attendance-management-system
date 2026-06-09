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
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    // Model Configurations    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AttendanceDbContext).Assembly);

        ConfigureBaseEntity(modelBuilder);

        ApplyGlobalFilters(modelBuilder);

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