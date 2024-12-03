namespace SurveyBasket.Api.Abstractions
{
    //code => unqiue identifier for each error 
    //Description => Describe the error
    public record Error(string Code,string Description)
    {
        public static readonly Error None = new(string.Empty , string.Empty);
    }
}
