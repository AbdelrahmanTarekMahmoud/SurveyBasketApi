using SurveyBasket.Api.Contracts.Polls;

namespace SurveyBasket.Api.Contracts.Authentication
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().MaximumLength(100).EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MaximumLength(100);

        }
        
    }
}
