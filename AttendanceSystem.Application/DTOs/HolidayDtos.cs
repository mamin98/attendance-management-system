using AttendanceSystem.Domain;
namespace AttendanceSystem.Application;

public class HolidayDto : HolidayBaseDto
{
    public Guid Id { get; set; }
    public Guid? DepartmentId { get; set; }
    public DepartmentSimpleDto? DepartmentData { get; set; }
}

public class CreateHolidayDto : HolidayBaseDto
{
    public Guid? DepartmentId { get; set; }

    public Holiday ToEntity() 
    => Holiday.Create(NameEnglish, NameArabic, StartDate, EndDate, DepartmentId);
}

public class UpdateHolidayDto : CreateHolidayDto
{
    public Guid Id { get; set; }

    public Holiday UpdateEntity(Holiday entity) 
    => entity.Update(NameEnglish, NameArabic, StartDate, EndDate, DepartmentId);
}

public class HolidaySearchDto : SearchDto
{
    public Guid? DepartmentId { get; set; }
}

public class HolidayBaseDto
{
    public string NameEnglish { get; set; } = string.Empty;
    public string NameArabic { get; set; } = string.Empty;
    public string StartDate { get; set; } = string.Empty;
    public string EndDate { get; set; } = string.Empty;
}