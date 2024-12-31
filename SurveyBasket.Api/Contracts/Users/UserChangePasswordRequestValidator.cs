namespace SurveyBasket.Api.Contracts.Users
{
    public class UserChangePasswordRequestValidator : AbstractValidator<UserChangePasswordRequest>
    {
        public UserChangePasswordRequestValidator()
        {
            RuleFor(x => x.CurrentPassword).NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty()
                .Matches(RegexPatterns.Password).WithMessage("Password is weak")
                .NotEqual(x => x.CurrentPassword)
                .WithMessage("can not change your password to current password");
        }
    }
}
