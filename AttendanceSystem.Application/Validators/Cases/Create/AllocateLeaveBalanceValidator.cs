using FluentValidation;

namespace AttendanceSystem.Application;

public class AllocateLeaveBalanceValidator : AbstractValidator<AllocateLeaveBalanceDto>
{
    public AllocateLeaveBalanceValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithMessage("Employee is required.");

        RuleFor(x => x.LeaveTypeId)
            .NotEmpty()
            .WithMessage("Leave type is required.");

        RuleFor(x => x.Year)
            .InclusiveBetween(2000, 2100)
            .WithMessage("Year must be between 2000 and 2100.");

        RuleFor(x => x.AllocatedDays)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Allocated days cannot be negative.");
    }
}