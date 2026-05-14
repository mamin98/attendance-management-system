using AttendanceSystem.Domain;
using FluentValidation;

namespace AttendanceSystem.Application;

public class CreateAttendanceRequestValidator
    : AbstractValidator<CreateAttendanceRequestDto>
{
    public CreateAttendanceRequestValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithMessage("Employee is required");

        RuleFor(x => x.RequestType)
            .IsInEnum()
            .WithMessage("Invalid request type");

        RuleFor(x => x.RequestDate)
            .NotEmpty()
            .WithMessage("Request date is required");

        RuleFor(x => x.RequestDate)
            .Must(date => date.Date >= DateTime.Today.AddDays(-7))
            .WithMessage("Request date cannot be too old");

        RuleFor(x => x.Reason)
            .MaximumLength(500)
            .WithMessage("Reason cannot exceed 500 characters");

        RuleFor(x => x)
            .Must(HaveValidTimeRange)
            .WithMessage("FromTime must be less than ToTime");

        RuleFor(x => x)
            .Must(HaveBothTimesOrNone)
            .WithMessage("FromTime and ToTime must both have values");
    }

    private static bool HaveValidTimeRange(
        CreateAttendanceRequestDto dto)
    {
        if (!dto.FromTime.HasValue || !dto.ToTime.HasValue)
            return true;

        return dto.FromTime < dto.ToTime;
    }

    private static bool HaveBothTimesOrNone(
        CreateAttendanceRequestDto dto)
    {
        return dto.FromTime.HasValue == dto.ToTime.HasValue;
    }
}