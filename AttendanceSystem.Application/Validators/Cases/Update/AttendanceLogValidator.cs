using FluentValidation;

namespace AttendanceSystem.Application;

public class UpdateAttendanceLogValidator : AbstractValidator<UpdateAttendanceLogDto>
{
    public UpdateAttendanceLogValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.CheckIn)
            .Must(BeValidTime)
            .When(x => !string.IsNullOrWhiteSpace(x.CheckIn))
            .WithMessage("Invalid check-in time.");

        RuleFor(x => x.CheckOut)
            .Must(BeValidTime)
            .When(x => !string.IsNullOrWhiteSpace(x.CheckOut))
            .WithMessage("Invalid check-out time.");

        RuleFor(x => x)
            .Must(HaveValidTimeOrder)
            .When(x => !string.IsNullOrWhiteSpace(x.CheckIn) &&
                       !string.IsNullOrWhiteSpace(x.CheckOut))
            .WithMessage("Check-out must be after check-in.");
    }

    private static bool BeValidTime(string? value)
        => TimeOnly.TryParse(value, out _);

    private static bool HaveValidTimeOrder(UpdateAttendanceLogDto dto)
    {
        return TimeOnly.TryParse(dto.CheckIn, out var checkIn) &&
               TimeOnly.TryParse(dto.CheckOut, out var checkOut) &&
               checkOut > checkIn;
    }
}