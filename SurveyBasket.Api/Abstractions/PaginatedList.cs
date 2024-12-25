namespace SurveyBasket.Api.Abstractions
{
    //generic because i will return various types of data
    //depends on the request polls,questions ... 
    public class PaginatedList<T>
    {
        //count is the number of total items
        public PaginatedList(List<T> items , int pageNumber , int count , int PageSize)
        {
            PageItems = items;
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)PageSize);
        }
        //Total items in each page
        public List<T> PageItems { get; private set; }
        public int PageNumber {  get; private set; }
        //To help frontend to shape the pages 
        public int TotalPages { get; private set;}
        //starts with 1 
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;

        public static async Task<PaginatedList<T>> CreateListAsync(IQueryable<T> query , int pageNumber , int pageSize , CancellationToken cancellationToken = default)
        {
            //count number of items
            var count = await query.CountAsync(cancellationToken);
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return new PaginatedList<T>(items, pageNumber, count, pageSize);
        }
    }
}
