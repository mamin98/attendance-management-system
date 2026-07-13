using FluentValidation;

namespace AttendanceSystem.Application;

public class GenerateReportRequestValidator : AbstractValidator<GenerateReportRequestDto>
{
    public GenerateReportRequestValidator()
    {
        RuleFor(x => x.Month)
            .InclusiveBetween(1, 12)
            .WithMessage("Month must be between 1 and 12.");

        RuleFor(x => x.Year)
            .InclusiveBetween(2000, 2100)
            .WithMessage("Year must be between 2000 and 2100.");

        RuleFor(x => x.EmployeeId)
            .NotEqual(Guid.Empty)
            .When(x => x.EmployeeId.HasValue);

        RuleFor(x => x.DepartmentId)
            .NotEqual(Guid.Empty)
            .When(x => x.DepartmentId.HasValue);

    }
}