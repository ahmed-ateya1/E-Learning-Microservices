using FluentValidation;
using WishlistManagementMicroservice.BusinessLayer.Dtos;

namespace WishlistManagementMicroservice.BusinessLayer.Validators
{
    public class WishlistAddRequestValidator : AbstractValidator<WishlistAddRequest>
    {
        public WishlistAddRequestValidator()
        {
            //RuleFor(x=>x.UserID)
            //    .NotEmpty().WithMessage("UserID is required");
            //RuleFor(x => x.CourseID)
            //    .NotEmpty().WithMessage("CourseID is required");
        }
    }
}
