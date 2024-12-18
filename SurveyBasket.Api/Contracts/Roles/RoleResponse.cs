namespace SurveyBasket.Api.Contracts.Roles
{
    public record RoleResponse
    (
        string Id,
        string Name,
        bool IsDeleted // to be used in toggle
    );
}
