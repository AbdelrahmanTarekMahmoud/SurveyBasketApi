namespace SurveyBasket.Api.Contracts.Roles
{
    public class RoleRequestValidator : AbstractValidator<RolesRequest>
    {
        public RoleRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(3, 256);
            RuleFor(x => x.Permissions).NotEmpty().NotNull();
            RuleFor(x => x.Permissions).Must(x => x.Distinct().Count() == x.Count).
                WithMessage("Cannot Duplicate The Same Permission")
                .When(x => x.Permissions != null);
        }
    }
}
