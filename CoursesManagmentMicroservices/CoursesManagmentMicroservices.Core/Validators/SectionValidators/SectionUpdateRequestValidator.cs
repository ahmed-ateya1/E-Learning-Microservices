using CoursesManagmentMicroservices.Core.Dtos.SectionDto;
using FluentValidation;

namespace CoursesManagmentMicroservices.Core.Validators.SectionValidators
{
    public class SectionUpdateRequestValidator : AbstractValidator<SectionUpdateRequest>
    {
        public SectionUpdateRequestValidator()
        {
            RuleFor(x => x.SectionID)
                .NotEmpty().WithMessage("SectionID is required");
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100).WithMessage("Title can't be more than 100 characters");
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description can't be more than 500 characters");
            RuleFor(x => x.Order)
                .NotEmpty().WithMessage("Order is required")
                .GreaterThan(0).WithMessage("Order must be greater than 0");
            RuleFor(x => x.CourseID)
                .NotEmpty().WithMessage("CourseID is required");
        }
    }
}
