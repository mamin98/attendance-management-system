using AttendanceSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Infrastructure;

public class DataSeeder
{
    private readonly AttendanceDbContext _context;

    public DataSeeder(AttendanceDbContext context) => _context = context;

    public async Task SeedAsync()
    {
        if (await _context.Employees.AnyAsync())
            return;

        Guid hrManagerId = Guid.NewGuid();
        Guid itManagerId = Guid.NewGuid();
        Guid finManagerId = Guid.NewGuid();

        Guid hrDeptId = Guid.NewGuid();
        Guid itDeptId = Guid.NewGuid();
        Guid finDeptId = Guid.NewGuid();

        Guid emp1 = Guid.NewGuid();
        Guid emp2 = Guid.NewGuid();
        Guid emp3 = Guid.NewGuid();

        Guid att1 = Guid.NewGuid();
        Guid att2 = Guid.NewGuid();

        
        Employee hrManager = Employee.Create(
            hrDeptId,
            EmployeeRole.Manager,
            "Sara Ahmed",
            "سارة أحمد",
            "sara.hr@company.com");

        hrManager.SetId(hrManagerId);

        Employee itManager = Employee.Create(
            itDeptId,
            EmployeeRole.Manager,
            "Omar Khaled",
            "عمر خالد",
            "omar.it@company.com");

        itManager.SetId(itManagerId);

        Employee finManager = Employee.Create(
            finDeptId,
            EmployeeRole.Manager,
            "Mona Hassan",
            "منى حسن",
            "mona.finance@company.com");

        finManager.SetId(finManagerId);

        Employee employee1 = Employee.Create(
            itDeptId,
            EmployeeRole.Employee,
            "Ali Mohamed",
            "علي محمد",
            "ali.m@company.com");

        employee1.SetId(emp1);

        Employee employee2 = Employee.Create(
            itDeptId,
            EmployeeRole.Employee,
            "Youssef Tarek",
            "يوسف طارق",
            "youssef.t@company.com");

        employee2.SetId(emp2);
        

        Employee employee3 = Employee.Create(
            hrDeptId,
            EmployeeRole.Employee,
            "Nour Samir",
            "نور سمير",
            "nour.s@company.com");

        employee3.SetId(emp3);

        Employee admin = Employee.Create(
            itDeptId,
            EmployeeRole.Admin,
            "System Admin",
            "مسؤول النظام",
            "admin@company.com");

        admin.SetId(Guid.NewGuid());


        Department hrDept = Department.Create(
            hrManagerId,
            "Human Resources",
            "الموارد البشرية");

        hrDept.SetId(hrDeptId);

        Department itDept = Department.Create(
            itManagerId,
            "Information Technology",
            "تكنولوجيا المعلومات");

        itDept.SetId(itDeptId);

        Department finDept = Department.Create(
            finManagerId,
            "Finance",
            "المالية");

        finDept.SetId(finDeptId);

        AttendanceRequest attRequest1 = AttendanceRequest.Create(
            emp1,
            RequestType.Late,
            DateTime.UtcNow.Date,
            new TimeSpan(9, 0, 0),
            new TimeSpan(10, 0, 0),
            "Traffic delay");

        attRequest1.SetId(att1);

        AttendanceRequest attRequest2 = AttendanceRequest.Create(
            emp2,
            RequestType.Remote,
            DateTime.UtcNow.Date.AddDays(-1),
            null,
            null,
            "Working from home");

        attRequest2.SetId(att2);
        attRequest2.Approve(); 

        await _context.Employees.AddRangeAsync(
            admin,
            hrManager, itManager, finManager,
            employee1, employee2, employee3
        );

        await _context.Departments.AddRangeAsync(
            hrDept, itDept, finDept
        );

        await _context.AttendanceRequests.AddRangeAsync(
            attRequest1, attRequest2
        );

        await _context.SaveChangesAsync();
    }
}
