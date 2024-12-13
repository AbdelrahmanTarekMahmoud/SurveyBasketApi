namespace SurveyBasket.Api.Contracts.Users
{
    //User only can change his FirstName &  LastName
    public record UserProfileUpdateRequest
    (
        string FirstName,
        string LastName
        );
}
