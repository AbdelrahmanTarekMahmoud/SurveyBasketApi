namespace SurveyBasket.Api.Abstractions
{
    //code => unqiue identifier for each error 
    //Description => Describe the error
    public record Error(string Code,string Description , int? StatusCode)
    {
        public static readonly Error None = new(string.Empty , string.Empty , null);
    }
}
