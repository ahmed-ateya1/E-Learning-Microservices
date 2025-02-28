using CoursesManagmentMicroservices.Core.Dtos.SectionDto;
using FluentValidation;

namespace CoursesManagmentMicroservices.Core.Validators.SectionValidators
{
    public class SectionAddRequestValidator : AbstractValidator<SectionAddRequest>
    {
        public SectionAddRequestValidator()
        {
            RuleFor(x=>x.Title)
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
