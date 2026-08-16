using FluentValidation;

namespace AttendanceSystem.Application;

public class CreateAttendanceLogValidator : AbstractValidator<CreateAttendanceLogDto>
{
    public CreateAttendanceLogValidator()
    {
        RuleFor(x => x.EmployeeId).NotEmpty();

        RuleFor(x => x.Date).NotEmpty();

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

    private static bool HaveValidTimeOrder(CreateAttendanceLogDto dto)
    {
        return TimeOnly.TryParse(dto.CheckIn, out var checkIn) &&
               TimeOnly.TryParse(dto.CheckOut, out var checkOut) &&
               checkOut > checkIn;
    }
}