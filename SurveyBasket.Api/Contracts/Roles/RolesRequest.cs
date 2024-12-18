namespace SurveyBasket.Api.Contracts.Roles
{
    public record RolesRequest
    (
        string Name,
        IList<string> Permissions
    );
}
