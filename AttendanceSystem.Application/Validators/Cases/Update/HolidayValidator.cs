using FluentValidation;

namespace AttendanceSystem.Application;

public class UpdateHolidayValidator : AbstractValidator<UpdateHolidayDto>
{
    public UpdateHolidayValidator()
    {
        Include(new CreateHolidayValidator());

        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
