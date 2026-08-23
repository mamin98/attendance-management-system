using AttendanceSystem.Domain;
using System.Globalization;
using FluentValidation;

namespace AttendanceSystem.Application;
public class AttendanceRequestBaseValidator<T> : AbstractValidator<T>
    where T : AttendanceRequestBaseDto
{
    public AttendanceRequestBaseValidator()
    {
        RuleFor(x => x.RequestDate)
            .NotEmpty()
            .WithMessage("Request date is required");

        RuleFor(x => x.RequestDate)
            .NotEmpty()
            .WithMessage("Request date is required")
            .Must(BeValidDate)
            .WithMessage("Request date must be in yyyy-MM-dd format")
            .Must(NotBeTooOld)
            .WithMessage("Request date cannot be older than 7 days");

        RuleFor(x => x.Reason)
            .MaximumLength(500)
            .WithMessage("Reason cannot exceed 500 characters");

        RuleFor(x => x.FromTime)
            .Must(BeValidTime)
            .When(x => !string.IsNullOrWhiteSpace(x.FromTime))
            .WithMessage("FromTime must be in HH:mm format");

        RuleFor(x => x.ToTime)
            .Must(BeValidTime)
            .When(x => !string.IsNullOrWhiteSpace(x.ToTime))
            .WithMessage("ToTime must be in HH:mm format");

        RuleFor(x => x)
            .Must(HaveBothTimesOrNone)
            .WithMessage("FromTime and ToTime must both have values");

        RuleFor(x => x)
            .Must(HaveValidTimeRange)
            .When(x => !string.IsNullOrWhiteSpace(x.FromTime) &&
                       !string.IsNullOrWhiteSpace(x.ToTime))
            .WithMessage("FromTime must be earlier than ToTime");
    }

    private static bool BeValidDate(string date)
    {
        return DateOnly.TryParseExact(
            date,
            AttendanceSystemConsts.DateTimeFormat,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out _);
    }

    private static bool NotBeTooOld(string date)
    {
        if (!DateOnly.TryParseExact(
                date,
                AttendanceSystemConsts.DateTimeFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var parsedDate))
        {
            return false;
        }

        return parsedDate >= DateOnly.FromDateTime(DateTime.Today.AddDays(-7));
    }

    private static bool BeValidTime(string? time)
    {
        return TimeOnly.TryParseExact(
            time,
            AttendanceSystemConsts.TimeFormat,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out _);
    }

    private static bool HaveBothTimesOrNone(AttendanceRequestBaseDto dto)
    {
        return string.IsNullOrWhiteSpace(dto.FromTime)
            == string.IsNullOrWhiteSpace(dto.ToTime);
    }

    private static bool HaveValidTimeRange(AttendanceRequestBaseDto dto)
    {
        if (!TimeOnly.TryParseExact(dto.FromTime, AttendanceSystemConsts.TimeFormat,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var from))
            return false;

        if (!TimeOnly.TryParseExact(dto.ToTime, AttendanceSystemConsts.TimeFormat,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var to))
            return false;

        return from < to;
    }
}