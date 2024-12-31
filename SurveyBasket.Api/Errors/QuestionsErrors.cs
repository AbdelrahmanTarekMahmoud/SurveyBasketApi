namespace SurveyBasket.Api.Errors
{
    public class QuestionsErrors
    {
        public static readonly Error QuestionNoFound =
         new("Question.QuestionNoFound", "There is no existing question ", StatusCodes.Status404NotFound);
        public static readonly Error DuplicatedQuestion =
         new("Question.DuplicatedQuestion", "There is a existing Question with the same content in the same Poll ", StatusCodes.Status409Conflict);
    }
}
