namespace SurveyBasket.Api.Abstractions.Constants
{
    public static class RegexPatterns
    {
        public const string Password = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&*(),.?\":{}|<>]).{8,}$";
    }
}
