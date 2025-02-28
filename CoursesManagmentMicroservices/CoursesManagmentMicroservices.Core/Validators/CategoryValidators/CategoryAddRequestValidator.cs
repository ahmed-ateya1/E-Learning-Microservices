using CoursesManagmentMicroservices.Core.Dtos.CategoryDto;
using FluentValidation;

namespace CoursesManagmentMicroservices.Core.Validators.CategoryValidators
{
    public class CategoryAddRequestValidator : AbstractValidator<CategoryAddRequest>
    {
        public CategoryAddRequestValidator()
        {
            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Category name is required")
                .MaximumLength(100).WithMessage("Category name can not be longer than 100 characters");

        }
    }

}
