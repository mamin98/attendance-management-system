using AttendanceSystem.Infrastructure;
using AttendanceSystem.Application;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();

            builder.Services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
    
    
            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                AttendanceDbContext dbContext = scope.ServiceProvider
                    .GetRequiredService<AttendanceDbContext>();

                await dbContext.Database.MigrateAsync();

                DataSeeder seeder = scope.ServiceProvider
                    .GetRequiredService<DataSeeder>();

                await seeder.SeedAsync();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionMiddleware>(); // apply first
            
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
