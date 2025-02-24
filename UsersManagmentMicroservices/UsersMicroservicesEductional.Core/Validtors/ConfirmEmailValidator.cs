using FluentValidation;
using UsersMicroservicesEductional.Core.Dtos.AuthenticationDto;

namespace UsersMicroservicesEductional.Core.Validtors
{
    public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailDTO>
    {
        public ConfirmEmailValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is not valid");
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required");
        }
    }
}
