using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public class ShiftDto : ShiftBaseDto
{
    public Guid Id { get; set; }
    public List<ShiftDayDto> Days { get; set; } = [];
}

public class CreateShiftDto : ShiftBaseDto
{
    public List<CreateShiftDayDto> Days { get; set; } = [];
    public Shift ToEntity() => Shift.Create(NameArabic, NameEnglish, StartDate, EndDate, GracePeriodMinutes);
}

public class UpdateShiftDto : CreateShiftDto
{
    public Guid Id { get; set; }

    public Shift UpdateEntity(Shift entity) => entity.Update(NameArabic, NameEnglish, StartDate, EndDate, GracePeriodMinutes);
}

public class ShiftSearchDto : SearchDto { }

public class ShiftBaseDto
{
    public required string NameArabic { get; set; }
    public required string NameEnglish { get; set; }
    public string StartDate { get; set; } = string.Empty;
    public string EndDate { get; set; } = string.Empty;
    public int GracePeriodMinutes { get; set; }
}