using FluentValidation;

namespace AttendanceSystem.Application;

public class CreateSalaryStructureValidator : AbstractValidator<CreateSalaryStructureDto>
{
    public CreateSalaryStructureValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithMessage("Employee is required.");

        RuleFor(x => x.BaseSalary)
            .GreaterThan(0)
            .WithMessage("Base salary must be greater than zero.");

        RuleFor(x => x.LateDeductionPerMinute)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Late deduction per minute cannot be negative.");

        RuleFor(x => x.AbsenceDeductionPerDay)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Absence deduction per day cannot be negative.");
    }
}