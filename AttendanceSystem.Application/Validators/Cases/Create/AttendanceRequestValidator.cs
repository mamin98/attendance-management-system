using FluentValidation;

namespace AttendanceSystem.Application;

public class CreateAttendanceRequestValidator
    : AbstractValidator<CreateAttendanceRequestDto>
{
    public CreateAttendanceRequestValidator()
    {
        Include(new AttendanceRequestBaseValidator<CreateAttendanceRequestDto>());

        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithMessage("Employee is required");

        RuleFor(x => x.RequestType)
            .IsInEnum()
            .WithMessage("Invalid request type");
    }
}