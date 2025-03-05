using FluentValidation;
using ReviewManagementMicroservice.Core.Dtos;

namespace ReviewManagementMicroservice.Core.Validators
{
    public class ReviewUpdateRequestValidator : AbstractValidator<ReviewUpdateRequest>
    {
        public ReviewUpdateRequestValidator()
        {
            RuleFor(x=>x.ReviewID)
                .NotEmpty()
                .WithMessage("Review ID is required");
            RuleFor(x => x.ReviewText)
                .NotEmpty().WithMessage("Review text is required");

            RuleFor(x => x.ReviewText)
                .MaximumLength(500)
                .WithMessage("Review text can't be longer than 500 characters");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5)
                .WithMessage("Rating must be between 1 and 5");

            RuleFor(x => x.UserID)
                .NotEmpty()
                .WithMessage("User ID is required");
            RuleFor(x => x.CourseID)
                .NotEmpty()
                .WithMessage("Course ID is required");
        }
    }
}
