namespace SurveyBasket.Api.Errors
{
    public static class UserErrors
    {
        public static readonly Error InvalidCredentials =
         new("User.InvalidCredentials", "Invalid UserName / Password", StatusCodes.Status401Unauthorized);

        public static readonly Error AlreadyRegisteredEmail =
         new("User.AlreadyRegisteredEmail", "This Email already registered", StatusCodes.Status409Conflict);

        public static readonly Error EmailNotConfirmed =
         new("User.EmailNotConfirmed", "Email is not confirmed", StatusCodes.Status401Unauthorized);

        public static readonly Error InvalidConfirmationCode =
         new("User.InvalidConfirmationCode", "the code you entered is invalid", StatusCodes.Status401Unauthorized);

        public static readonly Error UserAlreadyConfirmed =
         new("User.UserAlreadyConfirmed", "This user is already confirmed", StatusCodes.Status401Unauthorized);

        public static readonly Error UserDoesnotExist =
         new("User.UserDoesnotExist", "This user is already confirmed", StatusCodes.Status409Conflict);

        public static readonly Error InvalidCode =
         new("User.InvalidCode", "The code you entered is wrong", StatusCodes.Status400BadRequest);

        public static readonly Error UserIsDisabled =
         new("User.UserIsDisabled", "User is Disabled", StatusCodes.Status401Unauthorized);

        public static readonly Error UserIsLockedOut =
         new("User.UserIsLockedOut", "User is Locked Out try again later!", StatusCodes.Status401Unauthorized);

        public static readonly Error UserNotFound =
         new("User.UserNotFound", "User  is not exist", StatusCodes.Status401Unauthorized);
    }
}
