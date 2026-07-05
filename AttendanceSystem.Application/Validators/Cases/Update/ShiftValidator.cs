using FluentValidation;

namespace AttendanceSystem.Application;

public class UpdateShiftValidator : AbstractValidator<UpdateShiftDto>
{
    public UpdateShiftValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.NameEnglish).NotEmpty().MaximumLength(100);
        RuleFor(x => x.NameArabic).NotEmpty().MaximumLength(100);
        RuleFor(x => x.GracePeriodMinutes).InclusiveBetween(0, 120);

        RuleFor(x => x.Days).NotEmpty().WithMessage("A shift must have at least one day configured");

        RuleForEach(x => x.Days).SetValidator(new CreateShiftDayValidator());
    }
}