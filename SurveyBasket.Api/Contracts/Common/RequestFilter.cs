namespace SurveyBasket.Api.Contracts.Common
{
    public record RequestFilter
    {
        //Pagination
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        //Searching
        public string? SearchItem { get; init; }
        //Sorting
        public string? SortedCol { get; init; } //name of col which will be sorted
        public string? SortingType { get; init; } = "ASC"; //Asc or Dest
    }
}
