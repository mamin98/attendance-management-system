using FluentValidation;

namespace AttendanceSystem.Application;
public class CreateShiftDayDetailValidator : AbstractValidator<CreateShiftDayDetailDto>
{
    public CreateShiftDayDetailValidator()
    {
        RuleFor(x => x.FromTime)
            .NotEmpty()
            .WithMessage("From time is required")
            .Must(BeValidTime)
            .WithMessage("From time must be a valid time.");

        RuleFor(x => x.ToTime)
            .NotEmpty()
            .WithMessage("To time is required")
            .Must(BeValidTime)
            .WithMessage("To time must be a valid time.");

        RuleFor(x => x)
            .Must(HaveValidTimeRange)
            .WithMessage("To time must be later than From time.");
    }

    private static bool BeValidTime(string time)
        => TimeOnly.TryParse(time, out _);

    private static bool HaveValidTimeRange(CreateShiftDayDetailDto dto)
    {
        if (!TimeOnly.TryParse(dto.FromTime, out var fromTime) ||
            !TimeOnly.TryParse(dto.ToTime, out var toTime))
        {
            return false;
        }

        return toTime > fromTime;
    }
}