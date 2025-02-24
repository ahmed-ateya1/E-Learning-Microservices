using FluentValidation;
using UsersMicroservicesEductional.Core.Dtos.AuthenticationDto;

namespace UsersMicroservicesEductional.Core.Validtors
{
    public class RevokTokenValidator : AbstractValidator<RevokTokenDTO>
    {
        public RevokTokenValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token is required");
        }
    }
}
