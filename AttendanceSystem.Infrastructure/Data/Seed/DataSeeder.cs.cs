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

        
        // IDs
        
        Guid hrDeptId = Guid.NewGuid();
        Guid itDeptId = Guid.NewGuid();
        Guid finDeptId = Guid.NewGuid();

        Guid hrManagerId = Guid.NewGuid();
        Guid itManagerId = Guid.NewGuid();
        Guid finManagerId = Guid.NewGuid();

        Guid emp1Id = Guid.NewGuid();
        Guid emp2Id = Guid.NewGuid();
        Guid emp3Id = Guid.NewGuid();

                
        Department hrDept = Department.Create(null, "HR", "الموارد");
        hrDept.SetId(hrDeptId);

        Department itDept = Department.Create(null, "IT", "تكنولوجيا المعلومات");
        itDept.SetId(itDeptId);

        Department finDept = Department.Create(null, "Finance", "المالية");
        finDept.SetId(finDeptId);

        await _context.Departments.AddRangeAsync(hrDept, itDept, finDept);
        await _context.SaveChangesAsync();
        
        
        Employee hrManager = Employee.Create(EmployeeRole.Manager, "Sara", "سارة", "hr@c.com");
        hrManager.SetId(hrManagerId);

        Employee itManager = Employee.Create(EmployeeRole.Manager, "Omar", "عمر", "it@c.com");
        itManager.SetId(itManagerId);

        Employee finManager = Employee.Create(EmployeeRole.Manager, "Mona", "منى", "fin@c.com");
        finManager.SetId(finManagerId);

        await _context.Employees.AddRangeAsync(hrManager, itManager, finManager);
        await _context.SaveChangesAsync();

                
        hrDept.SetManagerId(hrManager.Id);
        itDept.SetManagerId(itManager.Id);
        finDept.SetManagerId(finManager.Id);

        _context.Departments.UpdateRange(hrDept, itDept, finDept);
        await _context.SaveChangesAsync();

                
        Employee emp1 = Employee.Create(EmployeeRole.Employee, "Ali", "علي", "a@c.com");
        emp1.SetId(emp1Id);

        Employee emp2 = Employee.Create(EmployeeRole.Employee, "Youssef", "يوسف", "y@c.com");
        emp2.SetId(emp2Id);

        Employee emp3 = Employee.Create(EmployeeRole.Employee, "Nour", "نور", "n@c.com");
        emp3.SetId(emp3Id);

        Employee admin = Employee.Create(EmployeeRole.Admin, "Admin", "مدير النظام", "admin@c.com");
        admin.SetId(Guid.NewGuid());

        await _context.Employees.AddRangeAsync(emp1, emp2, emp3, admin);
        await _context.SaveChangesAsync();

                
        await _context.Set<EmployeeDepartment>().AddRangeAsync(
            EmployeeDepartment.Create(emp1.Id, itDept.Id),
            EmployeeDepartment.Create(emp2.Id, itDept.Id),
            EmployeeDepartment.Create(emp3.Id, hrDept.Id),
            EmployeeDepartment.Create(admin.Id, finDept.Id)
        );

        await _context.SaveChangesAsync();

                
        AttendanceRequest att1 = AttendanceRequest.Create(
            emp1.Id,
            RequestType.Late,
            DateTime.UtcNow.Date,
            new TimeSpan(9, 0, 0),
            new TimeSpan(10, 0, 0),
            "Traffic"
        );

        AttendanceRequest att2 = AttendanceRequest.Create(
            emp2.Id,
            RequestType.Remote,
            DateTime.UtcNow.Date.AddDays(-1),
            null,
            null,
            "WFH"
        );

        att2.Approve();

        await _context.AttendanceRequests.AddRangeAsync(att1, att2);
        await _context.SaveChangesAsync();
    }
}
