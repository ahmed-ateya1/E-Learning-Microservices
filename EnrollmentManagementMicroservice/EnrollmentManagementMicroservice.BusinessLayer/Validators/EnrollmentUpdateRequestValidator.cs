using EnrollmentManagementMicroservice.BusinessLayer.Dtos;
using FluentValidation;

namespace EnrollmentManagementMicroservice.BusinessLayer.Validators
{
    public class EnrollmentUpdateRequestValidator : AbstractValidator<EnrollmentUpdateRequest>
    {
        public EnrollmentUpdateRequestValidator()
        {
            RuleFor(x=>x.EnrollmentID).
                NotEmpty().WithMessage("EnrollmentID is required");

            RuleFor(x => x.UserID).
                NotEmpty().WithMessage("UserID is required");
            RuleFor(x => x.CourseID).
                NotEmpty().WithMessage("CourseID is required");

        }
    }
}
