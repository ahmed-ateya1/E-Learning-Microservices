using FluentValidation;
using QuizManagementMicroservice.Core.Dtos.QuizzDto;

namespace QuizManagementMicroservice.Core.Validators
{
    public class QuizzUpdateRequestValidator : AbstractValidator<QuizzUpdateRequest>
    {
        public QuizzUpdateRequestValidator()
        {
            RuleFor(x=>x.QuizzID)
                .NotEmpty().WithMessage("QuizzID is required");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters");

            RuleFor(x => x.TotalMarks)
                .NotEmpty().WithMessage("TotalMarks is required")
                .GreaterThan(0).WithMessage("TotalMarks must be greater than 0");

            RuleFor(x => x.PassMarks)
                .NotEmpty().WithMessage("PassMarks is required")
                .GreaterThan(0).WithMessage("PassMarks must be greater than or equal to 0");

            RuleFor(x => x.LectureID)
                .NotEmpty().WithMessage("LectureID is required");

            RuleFor(x => x.InstructorID)
                .NotEmpty().WithMessage("InstructorID is required");
        }
    }
}
