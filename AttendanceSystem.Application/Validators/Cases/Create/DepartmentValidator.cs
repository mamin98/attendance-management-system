using FluentValidation;

namespace AttendanceSystem.Application;

public class CreateDepartmentValidator : AbstractValidator<CreateDepartmentDto>
{
    public CreateDepartmentValidator()
    {
        RuleFor(x => x.NameEnglish)
            .NotEmpty().WithMessage("English name is required")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

        RuleFor(x => x.NameArabic)
            .NotEmpty().WithMessage("Arabic name is required")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");
    }
}