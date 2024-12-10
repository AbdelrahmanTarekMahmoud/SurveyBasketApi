namespace SurveyBasket.Api.Errors
{
    public static class UserErrors
    {
        public static readonly Error  InvalidCredentials = 
         new("User.InvalidCredentials", "Invalid UserName / Password" , StatusCodes.Status401Unauthorized);

        public static readonly Error AlreadyRegisteredEmail =
         new("User.AlreadyRegisteredEmail", "This Email already registered", StatusCodes.Status409Conflict);

        public static readonly Error EmailNotConfirmed =
         new("User.EmailNotConfirmed", "Email is not confirmed", StatusCodes.Status401Unauthorized);

        public static readonly Error InvalidConfirmationCode =
         new("User.InvalidConfirmationCode", "the code you entered is invalid", StatusCodes.Status401Unauthorized);

        public static readonly Error UserAlreadyConfirmed =
         new("User.UserAlreadyConfirmed", "This user is already confirmed", StatusCodes.Status401Unauthorized);

    }
}
