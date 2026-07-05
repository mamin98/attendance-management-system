using FluentValidation;

namespace AttendanceSystem.Application;

public class CreateShiftDayValidator : AbstractValidator<CreateShiftDayDto>
{
    public CreateShiftDayValidator()
    {
        RuleFor(x => x.Day).NotEmpty().WithMessage("Day is required");

        RuleFor(x => x.ShiftDayDetails)
            .Empty().WithMessage("Off days cannot have time details")
            .When(x => x.IsOffDay);

        RuleFor(x => x.ShiftDayDetails)
            .NotEmpty().WithMessage("At least one time range is required for a working day")
            .When(x => !x.IsOffDay);

        RuleForEach(x => x.ShiftDayDetails)
            .SetValidator(new CreateShiftDayDetailValidator())
            .When(x => !x.IsOffDay);
    }
}
