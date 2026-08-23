using FluentValidation;

namespace AttendanceSystem.Application;

public class UpdateAttendanceRequestValidator
    : AbstractValidator<UpdateAttendanceRequestDto>
{
    public UpdateAttendanceRequestValidator()
    {
        Include(new AttendanceRequestBaseValidator<UpdateAttendanceRequestDto>());

        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required");

        RuleFor(x => x.RequestType)
            .IsInEnum()
            .WithMessage("Invalid request type");
    }
}