using AttendanceSystem.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

        services
           .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(options =>
           {
               var jwtSettings =
                   configuration.GetSection("Jwt");

               options.TokenValidationParameters =
                   new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,

                       ValidIssuer = jwtSettings["Issuer"],
                       ValidAudience = jwtSettings["Audience"],

                       IssuerSigningKey =
                           new SymmetricSecurityKey(
                               Encoding.UTF8.GetBytes(
                                   jwtSettings["Key"]!))
                   };
           });

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddScoped<IEmployeeLeaveBalanceRepository, EmployeeLeaveBalanceRepository>();
        services.AddScoped<IEmployeeDepartmentRepository, EmployeeDepartmentRepository>();
        services.AddScoped<IAttendanceRequestRepository, AttendanceRequestRepository>();
        services.AddScoped<IAttendancePolicyRepository, AttendancePolicyRepository>();
        services.AddScoped<ISalaryStructureRepository, SalaryStructureRepository>();
        services.AddScoped<IShiftDayDetailRepository, ShiftDayDetailRepository>();
        services.AddScoped<IPayrollRecordRepository, PayrollRecordRepository>();
        services.AddScoped<IEmployeeShiftRepository, EmployeeShiftRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
        services.AddScoped<IAttachmentRepository, AttachmentRepository>();
        services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IShiftDayRepository, ShiftDayRepository>();
        services.AddScoped<IHolidayRepository, HolidayRepository>();
        services.AddScoped<IShiftRepository, ShiftRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();




        services.AddScoped<IAttendanceReportExportService, AttendanceReportExportService>();
        services.AddScoped<IAttendanceCalculationService, AttendanceCalculationService>();
        services.AddScoped<IEmployeeDepartmentService, EmployeeDepartmentService>();
        services.AddScoped<IAttendanceRequestService, AttendanceRequestService>();
        services.AddScoped<IAttendancePolicyService, AttendancePolicyService>();
        services.AddScoped<IAttendanceLogRepository, AttendanceLogRepository>();
        services.AddScoped<IAttendanceImportService, AttendanceImportService>();
        services.AddScoped<ISalaryStructureService, SalaryStructureService>();
        services.AddScoped<ILeaveRequestService, LeaveRequestService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IAttachmentService, AttachmentService>();
        services.AddScoped<ILeaveTypeService, LeaveTypeService>();
        services.AddScoped<IHolidayService, HolidayService>();
        services.AddScoped<IPayrollService, PayrollService>();
        services.AddScoped<IShiftService, ShiftService>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<IFileStorageService, LocalFileStorageService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
        services.AddScoped<IEmailService, EmailService>();


        services.AddScoped<DataSeeder>();
        services.AddHttpContextAccessor();

        return services;
    }

}