using FluentValidation;

namespace AttendanceSystem.Application;

public class CreateLeaveTypeValidator : AbstractValidator<CreateLeaveTypeDto>
{
    public CreateLeaveTypeValidator()
    {
        RuleFor(x => x.NameEnglish)
            .NotEmpty().WithMessage("English name is required.")
            .MaximumLength(100).WithMessage("English name cannot exceed 100 characters.");

        RuleFor(x => x.NameArabic)
            .NotEmpty().WithMessage("Arabic name is required.")
            .MaximumLength(100).WithMessage("Arabic name cannot exceed 100 characters.");

        RuleFor(x => x.DefaultDaysPerYear)
            .InclusiveBetween(0, 365)
            .WithMessage("Default days per year must be between 0 and 365.");
    }
}