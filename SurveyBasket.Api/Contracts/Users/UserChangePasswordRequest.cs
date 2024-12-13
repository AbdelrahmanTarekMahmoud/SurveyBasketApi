namespace SurveyBasket.Api.Contracts.Users
{
    public record UserChangePasswordRequest
    (
        string CurrentPassword,
        string NewPassword
    );
    
}
