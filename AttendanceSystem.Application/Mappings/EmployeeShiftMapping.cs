using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public static class EmployeeShiftMapping
{
    public static EmployeeShiftDto ToDto(this EmployeeShift entity) => new()
    {
        ShiftId = entity.ShiftId ?? Guid.Empty,
        ShiftNameEnglish = entity.Shift!.NameEnglish,
        ShiftNameArabic = entity.Shift.NameArabic,
        StartDate = entity.Shift?.StartDate.ToString() ?? string.Empty,
        EndDate = entity.Shift?.EndDate.ToString() ?? string.Empty,
    };
}