using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public static class ShiftMapping
{
    public static ShiftDto ToDto(this Shift entity) => new()
    {
        Id = entity.Id,
        NameEnglish = entity.NameEnglish,
        NameArabic = entity.NameArabic,
        StartDate = entity.StartDate.ToString() ?? string.Empty,
        EndDate = entity.EndDate.ToString() ?? string.Empty,
        GracePeriodMinutes = entity.GracePeriodMinutes,
        Days = [.. entity.ShiftDays
            .OrderBy(d => d.Day)
            .Select(d => new ShiftDayDto
            {
                Id = d.Id,
                Day = d.Day.ToString() ?? string.Empty,
                IsOffDay = d.IsOffDay,
                ShiftDayDetails = [.. (d.ShiftDayDetails ?? [])
                    .OrderBy(x => x.From)
                    .Select(x => new ShiftDayDetailDto { Id = x.Id, FromTime = x.From.ToString(), ToTime = x.To.ToString() })]
            })]
    };

}