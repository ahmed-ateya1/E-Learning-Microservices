using EnrollmentManagementMicroservice.BusinessLayer.Dtos;
using FluentValidation;

namespace EnrollmentManagementMicroservice.BusinessLayer.Validators
{
    public class EnrollmentAddRequestValidator : AbstractValidator<EnrollmentAddRequest>
    {
        public EnrollmentAddRequestValidator()
        {
            RuleFor(x=>x.UserID).
                NotEmpty().WithMessage("UserID is required");
            RuleFor(x => x.CourseID).
                NotEmpty().WithMessage("CourseID is required");

        }
    }
}
