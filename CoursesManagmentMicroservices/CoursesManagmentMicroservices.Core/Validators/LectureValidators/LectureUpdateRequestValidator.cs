using CoursesManagmentMicroservices.Core.Dtos.LectureDto;
using FluentValidation;

namespace CoursesManagmentMicroservices.Core.Validators.LectureValidators
{
    public class LectureUpdateRequestValidator : AbstractValidator<LectureUpdateRequest>
    {
        public LectureUpdateRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100).WithMessage("Title can't be more than 100 characters");
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description can't be more than 500 characters");
            RuleFor(x => x.Video);
            RuleFor(x => x.ResourceUrl)
                .MaximumLength(500).WithMessage("ResourceUrl can't be more than 500 characters");
            RuleFor(x => x.File);
            RuleFor(x => x.DurationInMinutes)
                .NotEmpty().WithMessage("DurationInMinutes is required")
                .GreaterThan(0).WithMessage("DurationInMinutes must be greater than 0");
            RuleFor(x => x.Order)
                .NotEmpty().WithMessage("Order is required")
                .GreaterThan(0).WithMessage("Order must be greater than 0");
            RuleFor(x => x.SectionID)
                .NotEmpty().WithMessage("SectionID is required");
            RuleFor(x=>x.LectureID)
                .NotEmpty().WithMessage("LectureID is required");
        }
    }
}
