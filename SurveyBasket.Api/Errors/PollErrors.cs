namespace SurveyBasket.Api.Errors
{
    public class PollErrors
    {
        public static readonly Error PollNotFound =
         new("Poll.PollNotFound", "There is no existing Poll with this Id");
    }
}

