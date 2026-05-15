using AttendanceSystem.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
 
namespace AttendanceSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AttendanceDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IAttendanceRequestRepository, AttendanceRequestRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAttendanceRequestService, AttendanceRequestService>();
        services.AddScoped<DataSeeder>();
        
        return services;
    }
    
}