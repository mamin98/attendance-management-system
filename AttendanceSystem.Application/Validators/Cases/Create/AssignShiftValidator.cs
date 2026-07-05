using FluentValidation;

namespace AttendanceSystem.Application;

public class AssignShiftValidator : AbstractValidator<AssignShiftDto>
{
    public AssignShiftValidator()
    {
        RuleFor(x => x.EmployeeId).NotEmpty();

        RuleFor(x => x.ShiftId).NotEmpty();

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .Must(BeValidDate)
            .WithMessage("Start Date must be a valid date.");

        RuleFor(x => x.EndDate)
            .Must(x => string.IsNullOrWhiteSpace(x) || BeValidDate(x))
            .WithMessage("End Date must be a valid date.");

        RuleFor(x => x)
            .Must(HaveValidDateRange)
            .WithMessage("End Date must be greater than or equal to Start Date.")
            .When(x => !string.IsNullOrWhiteSpace(x.EndDate));
    }

    private static bool BeValidDate(string date)
        => DateOnly.TryParse(date, out _);

    private static bool HaveValidDateRange(AssignShiftDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.EndDate))
            return true;

        return DateOnly.TryParse(dto.StartDate, out var startDate)
            && DateOnly.TryParse(dto.EndDate, out var endDate)
            && endDate >= startDate;
    }
}