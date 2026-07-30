using FluentValidation;

namespace AttendanceSystem.Application;

public class CreateHolidayValidator : AbstractValidator<CreateHolidayDto>
{
    public CreateHolidayValidator()
    {
        Include(new HolidayBaseValidator());

        RuleFor(x => x.DepartmentId)
            .NotEmpty()
            .When(x => x.DepartmentId.HasValue);

        RuleFor(x => x)
            .Must(HaveValidDateRange)
            .WithMessage("EndDate must be greater than or equal to StartDate.");
    }

    private static bool HaveValidDateRange(CreateHolidayDto dto)
    {
        if (!DateOnly.TryParse(dto.StartDate, out var start))
            return true; 

        if (!DateOnly.TryParse(dto.EndDate, out var end))
            return true; 

        return end >= start;
    }
}


public class HolidayBaseValidator : AbstractValidator<HolidayBaseDto>
{
    public HolidayBaseValidator()
    {
        RuleFor(x => x.NameEnglish)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.NameArabic)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .Must(x => DateOnly.TryParse(x, out _))
            .WithMessage("StartDate must be a valid date.");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .Must(x => DateOnly.TryParse(x, out _))
            .WithMessage("EndDate must be a valid date.");
    }
}